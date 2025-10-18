using System.Threading.Tasks;
using Core;

namespace Screens
{
    public interface IScreenWithViewModel
    {
        Task InitializeWithViewModel(object viewModel, UiController uiController, object parameter = null);
    }
}