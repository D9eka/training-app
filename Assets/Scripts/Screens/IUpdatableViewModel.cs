using Screens.Factories.Parameters;
using Screens.ViewModels;

namespace Screens
{
    public interface IUpdatableViewModel<in TParam> : IViewModel
        where TParam : IScreenParameter
    {
        void UpdateParameter(TParam param);
    }
}