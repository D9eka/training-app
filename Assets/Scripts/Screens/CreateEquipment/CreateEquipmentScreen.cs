using System;
using System.Threading.Tasks;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.CreateEquipment
{
    public class CreateEquipmentScreen : ScreenWithViewModel<CreateEquipmentViewModel>
    {
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private Toggle _hasQuantity;
        [SerializeField] private Toggle _hasWeight;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _backButton;

        public override async Task InitializeAsync(CreateEquipmentViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            Vm.CanSaveChanged += MarkDirtyOrRefresh;
            
            _nameInput.text = Vm.Name;
            _hasQuantity.isOn = Vm.HasQuantity;
            _hasWeight.isOn = Vm.HasWeight;

            _nameInput.onValueChanged.RemoveAllListeners();
            _nameInput.onValueChanged.AddListener(v => Vm.Name = v);
            _hasQuantity.onValueChanged.RemoveAllListeners();
            _hasQuantity.onValueChanged.AddListener(v => Vm.HasQuantity = v);
            _hasWeight.onValueChanged.RemoveAllListeners();
            _hasWeight.onValueChanged.AddListener(v => Vm.HasWeight = v);

            _saveButton.onClick.RemoveAllListeners();
            _saveButton.onClick.AddListener(OnSave);

            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => UIController.CloseScreen());
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