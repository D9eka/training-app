using System.Threading.Tasks;
using Core;
using Screens.ViewModels;

namespace Screens
{
    public interface IScreenWithViewModel
    {
        Task InitializeWithViewModel(IViewModel viewModel, UiController uiController, object parameter = null);
    }
}