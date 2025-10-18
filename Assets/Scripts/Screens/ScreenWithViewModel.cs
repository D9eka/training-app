using System;
using System.Threading.Tasks;
using Core;
using Screens.ViewModels;

namespace Screens
{
    public abstract class ScreenWithViewModel<TViewModel> : ReactiveScreen, IScreenWithViewModel
        where TViewModel : class, IViewModel
    {
        protected TViewModel Vm;
        protected UiController UIController;

        public virtual async Task InitializeAsync(TViewModel viewModel, UiController uiController, object parameter = null)
        {
            if (IsInitialized)
                return;

            Vm = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            UIController = uiController ?? throw new ArgumentNullException(nameof(uiController));

            await base.InitializeAsync(parameter);
        }

        async Task IScreenWithViewModel.InitializeWithViewModel(object viewModel, UiController ui, object parameter)
        {
            if (viewModel is not TViewModel typed)
                throw new ArgumentException($"Expected {typeof(TViewModel).Name}, got {viewModel?.GetType().Name}");
            await InitializeAsync(typed, ui, parameter);
        }
    }
}