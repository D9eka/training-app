using System.Threading.Tasks;
using Core;
using Screens.ViewModels;

namespace Screens
{
    public interface IScreenWithViewModel
    {
        IViewModel IVm { get; }
        Task InitializeWithViewModel(IViewModel viewModel, UiController uiController, object parameter = null);
    }
}