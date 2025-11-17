using System;
using Screens.Factories.Parameters;
using Screens.ViewModels;

namespace Screens
{
    public abstract class ScreenWithUpdatableViewModel<TViewModel, TParam> 
        : ScreenWithViewModel<TViewModel>, IScreenWithUpdatableViewModel
        where TViewModel : class, IViewModel, IUpdatableViewModel<TParam>
        where TParam : IScreenParameter
    {
        public void UpdateViewModelParameter(IScreenParameter parameter)
        {
            if (parameter == null)
            {
                Vm.UpdateParameter(default);
                return;
            }

            if (parameter is not TParam typedParam)
            {
                throw new ArgumentException(
                    $"Expected parameter of type {typeof(TParam).Name}, but received {parameter.GetType().Name}");
            }

            Vm.UpdateParameter(typedParam);
        }
    }
}
