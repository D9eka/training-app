using System;
using System.Threading.Tasks;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.CreateEquipment
{
    /// <summary>
    /// Экран создания оборудования.
    /// </summary>
    public class CreateEquipmentScreen : Screen
    {
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private Toggle _hasQuantity;
        [SerializeField] private Toggle _hasWeight;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _backButton;

        private CreateEquipmentViewModel _vm;

        public override async Task InitializeAsync(object parameter = null)
        {
            ViewModelFactory factory = DiContainer.Instance.Resolve<ViewModelFactory>() ?? throw new InvalidOperationException("ViewModelFactory not resolved");
            _vm = factory.Create<CreateEquipmentViewModel>(parameter);

            Subscribe(() => _vm.CanSaveChanged -= OnCanSaveChanged);
            _vm.CanSaveChanged += OnCanSaveChanged;

            _nameInput.text = _vm.Name;
            _hasQuantity.isOn = _vm.HasQuantity;
            _hasWeight.isOn = _vm.HasWeight;

            _nameInput.onValueChanged.RemoveAllListeners();
            _nameInput.onValueChanged.AddListener(value => _vm.Name = value);
            _hasQuantity.onValueChanged.RemoveAllListeners();
            _hasQuantity.onValueChanged.AddListener(value => _vm.HasQuantity = value);
            _hasWeight.onValueChanged.RemoveAllListeners();
            _hasWeight.onValueChanged.AddListener(value => _vm.HasWeight = value);

            _saveButton.onClick.RemoveAllListeners();
            _saveButton.onClick.AddListener(OnSave);

            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => DiContainer.Instance.Resolve<UiController>().CloseScreen());

            OnCanSaveChanged(_vm.CanSave);
            await base.InitializeAsync(parameter);
        }

        private void OnSave()
        {
            _vm.Save();
            DiContainer.Instance.Resolve<UiController>().CloseScreen();
        }

        private void OnCanSaveChanged(bool canSave)
        {
            _saveButton.interactable = canSave;
        }
    }
}