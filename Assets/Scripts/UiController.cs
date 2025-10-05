using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject _navigationBar;

    // Структура для хранения экрана и его параметров
    private class ScreenState
    {
        public GameObject Screen { get; set; }
        public bool NavBarActiveState { get; set; }
    }

    private Stack<ScreenState> _screensStack = new Stack<ScreenState>();

    public async void OpenScreen(GameObject screen, bool disableActiveScreen = true, bool navBarActiveState = true)
    {
        if (disableActiveScreen && _screensStack.Count > 0)
        {
            _screensStack.Peek().Screen.SetActive(false);
        }

        screen.SetActive(true);
        _screensStack.Push(new ScreenState
        {
            Screen = screen,
            NavBarActiveState = navBarActiveState
        });
        _navigationBar.SetActive(navBarActiveState);

        var screenBase = screen.GetComponent<ScreenBase>();
        if (screenBase != null)
        {
            await screenBase.InitializeAsync(ServiceLocator.Instance.GetScreenParameter());
        }
    }

    public void CloseScreen()
    {
        if (_screensStack.Count > 0)
        {
            var closingState = _screensStack.Pop();
            closingState.Screen.SetActive(false);

            if (_screensStack.Count > 0)
            {
                var previousState = _screensStack.Peek();
                previousState.Screen.SetActive(true);
                _navigationBar.SetActive(previousState.NavBarActiveState); // Восстановить состояние панели
            }
            else
            {
                _navigationBar.SetActive(false); // Если стек пуст, скрыть панель
            }
        }
    }
}