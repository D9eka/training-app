using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Screens;
using Screens.Factories.Parameters;
using Screens.ViewExercise;
using Screens.ViewModels;
using Utils;
using Screen = Screens.Screen;

namespace Core
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private GameObject _navigationBar;
        private readonly Stack<Screen> _stack = new();
        private ViewModelFactory _viewModelFactory;

        public void Initialize(ViewModelFactory factory) => _viewModelFactory = factory;

        public async void OpenScreen(ScreenType type, IScreenParameter param = null)
        {
            try
            {
                GameObject screenGo = DiContainer.Instance.ResolveNamed(type.ToString()) ?? 
                                      throw new ArgumentNullException(nameof(screenGo), $"Screen {type} not found");

                Screen screen = screenGo.GetComponent<Screen>() ??
                                throw new InvalidOperationException($"No Screen component on {type}");

                await HandleScreenTransition(screen);
                await InitializeScreenIfNeeded(screen, type, param);
                await screen.OnShowAsync();
                _stack.Push(screen);
                _navigationBar?.SetActive(screen.ShowNavBar);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void CloseScreen()
        {
            try
            {
                if (_stack.Count == 0)
                    return;

                Screen top = _stack.Pop();
                await top.OnHideAsync();
                top.gameObject.SetActive(false);

                if (_stack.Count > 0)
                {
                    Screen prev = _stack.Peek();
                    prev.gameObject.SetActive(true);
                    await prev.OnShowAsync();
                    _navigationBar?.SetActive(prev.ShowNavBar);
                }
                else
                {
                    _navigationBar?.SetActive(false);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task HandleScreenTransition(Screen next)
        {
            bool showPrev = next.ShowPreviousScreen;
            if (!showPrev && _stack.Count > 0)
            {
                Screen prev = _stack.Peek();
                await prev.OnHideAsync();
                prev.gameObject.SetActive(false);
            }

            next.gameObject.SetActive(true);
        }

        private async Task InitializeScreenIfNeeded(Screen screen, ScreenType type, IScreenParameter parameter)
        {
            if (screen is IScreenWithViewModel vmScreen)
            {
                if (screen.IsInitialized && vmScreen is ScreenWithViewModel<IUpdatableViewModel<IScreenParameter>> updatableScreen)
                {
                    updatableScreen.Vm.UpdateParameter(parameter);
                    return;
                }

                var vm = _viewModelFactory.CreateForScreen(type, parameter);
                await vmScreen.InitializeWithViewModel(vm, this, parameter);
            }
            else if (!screen.IsInitialized)
            {
                await screen.InitializeAsync(parameter);
            }
        }
    }
}
