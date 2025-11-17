using Screens.Factories.Parameters;

namespace Screens
{
    public interface IScreenWithUpdatableViewModel : IScreenWithViewModel
    {
        void UpdateViewModelParameter(IScreenParameter parameter);
    }
}
