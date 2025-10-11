using System;
using System.Collections.Generic;
using Screens;
using UnityEngine;
using Screen = Screens.Screen;

namespace Core
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private GameObject _navigationBar;

        private readonly Stack<Screen> _stack = new();

        public async void OpenScreen(ScreenType type, object parameter = null)
        {
            try
            {
                GameObject screenGo = DiContainer.Instance.ResolveNamed(type.ToString());
                if (screenGo == null) throw new ArgumentNullException(nameof(screenGo));
            
                Screen screen = screenGo.GetComponent<Screen>();
                bool showNavBar = screen.ShowNavBar;
                bool showPreviousScreen = screen.ShowPreviousScreen;

                if (!showPreviousScreen && _stack.Count > 0)
                    _stack.Peek().gameObject.SetActive(false);

                screenGo.SetActive(true);
                _stack.Push(screen);
                if (_navigationBar != null) 
                    _navigationBar.SetActive(showNavBar);

                await screen.InitializeAsync(parameter);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        public void CloseScreen()
        {
            if (_stack.Count == 0) 
                return;
            Screen top = _stack.Pop();
            top.gameObject.SetActive(false);

            if (_stack.Count > 0)
            {
                Screen prev = _stack.Peek();
                prev.gameObject.SetActive(true);
                _navigationBar.SetActive(prev.ShowNavBar);
            }
        }
    }
}