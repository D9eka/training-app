using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.Components.Equipment;

namespace Screens.CreateExercise
{
    public class CreateExerciseScreen : ScreenBase
    {
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TMP_InputField _descInput;
        [SerializeField] private Transform _equipmentListParent;
        [SerializeField] private EquipmentItem _equipmentItemPrefab;
        [SerializeField] private Button _addEquipmentButton;

        private CreateExerciseViewModel _vm;
        private List<EquipmentItem> _spawnedItems = new List<EquipmentItem>();

        public override async Task InitializeAsync(object parameter = null)
        {
            _vm = new CreateExerciseViewModel(ServiceLocator.Instance.DataService);
            _vm.ExerciseChanged += RefreshEquipmentList;
            _vm.CanSaveChanged += UpdateSaveButton;
            ServiceLocator.Instance.DataService.DataChanged += OnDataChanged;

            _nameInput.text = _vm.Name;
            _descInput.text = _vm.Description;
            _nameInput.onValueChanged.AddListener(value => _vm.Name = value);
            _descInput.onValueChanged.AddListener(value => _vm.Description = value);

            _addEquipmentButton.onClick.AddListener(OnAddEquipment);

            UpdateSaveButton(!string.IsNullOrEmpty(_vm.Name));
            RefreshEquipmentList();
            await Task.CompletedTask;
        }
        
        

        private void OnEnable()
        {
            if (_vm == null) return;
            _vm.ExerciseChanged += RefreshEquipmentList;
            _vm.CanSaveChanged += UpdateSaveButton;
            ServiceLocator.Instance.DataService.DataChanged += OnDataChanged;
            RefreshEquipmentList();
        }

        private void OnDisable()
        {
            _vm.ExerciseChanged -= RefreshEquipmentList;
            _vm.CanSaveChanged -= UpdateSaveButton;
            ServiceLocator.Instance.DataService.DataChanged -= OnDataChanged;
        }

        private void OnDataChanged()
        {
            _vm.Load();
        }

        private void UpdateSaveButton(bool canSave)
        {
            _saveButton.interactable = canSave;
        }

        private void RefreshEquipmentList()
        {
            // Очистка старых префабов
            foreach (EquipmentItem item in _spawnedItems)
            {
                Destroy(item.gameObject);
            }
            _spawnedItems.Clear();
        
            // Спавн префабов для всех AllEquipments
            foreach (Equipment eq in _vm.AllEquipments)
            {
                ExerciseEquipment reqEq = _vm.RequiredEquipment.Find(r => r.Equipment == eq);
                int quantity = reqEq != null ? reqEq.Quantity : 0;

                EquipmentItem item = Instantiate(_equipmentItemPrefab, _equipmentListParent);
                item.Setup(eq, EquipmentItem.Mode.Create, quantity, OnRemoveEquipment, OnQuantityChanged);
                _spawnedItems.Add(item);
            }
        }

        private void OnAddEquipment()
        {
            UiController.OpenScreen(ServiceLocator.Instance.CreateEquipmentScreen.gameObject, false, false);
        }

        private void OnRemoveEquipment(Equipment eq)
        {
            _vm.RemoveEquipment(eq);
        }

        private void OnQuantityChanged(Equipment eq, int quantity)
        {
            _vm.UpdateEquipmentQuantity(eq, quantity);
        }

        protected override void OnSaveClicked()
        {
            _vm.Save();
            UiController.CloseScreen();
        }
    }
}