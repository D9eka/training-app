using System.Globalization;
using System.Threading.Tasks;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.AddWeight
{
    public class AddWeightScreen : ScreenWithViewModel<AddWeightViewModel>
    {
        [SerializeField] private TMP_InputField _weightInput;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _backButton;
        
        public override async Task InitializeAsync(AddWeightViewModel viewModel, UiController uiController, 
            object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);
            
            Vm.DataUpdated += MarkDirtyOrRefresh;
            
            _weightInput.text = Vm.Weight.ToString(CultureInfo.CurrentCulture);

            _weightInput.contentType = TMP_InputField.ContentType.DecimalNumber;
            _weightInput.onEndEdit.RemoveAllListeners();
            _weightInput.onEndEdit.AddListener(v => Vm.Weight = float.Parse(v, CultureInfo.CurrentCulture));

            _saveButton.onClick.RemoveAllListeners();
            _saveButton.onClick.AddListener(OnSave);

            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => UIController.CloseScreen());
            
            Refresh();
        }
        
        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                _saveButton.interactable = Vm.CanSave;
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void OnSave()
        {
            Vm.Save();
            UIController.CloseScreen();
        }
    }
}