using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Models;
using Screens.Factories.Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.Components;

namespace Screens.CreateExercise
{
    public class CreateExerciseScreen : ScreenWithUpdatableViewModel<CreateExerciseViewModel, ExerciseIdParameter>
    {
        [SerializeField] private TMP_Text _header;
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TMP_InputField _descInput;
        [SerializeField] private Transform _equipmentListParent;
        [SerializeField] private EquipmentItem _equipmentPrefab;
        [SerializeField] private Button _createButton;
        [SerializeField] private TMP_Text _createButtonText;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _addEquipmentButton;
        
        private ItemsGroup<EquipmentItem> _equipmentItemsGroup;

        public override async Task InitializeAsync(CreateExerciseViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            _equipmentItemsGroup = new ItemsGroup<EquipmentItem>(_equipmentListParent, _equipmentPrefab);
            
            Vm.EditModeChanged += OnEditModeChanged;
            Vm.CanSaveChanged += OnCanSaveChanged;
            Vm.EquipmentsChanged += MarkDirtyOrRefresh;

            Subscribe(() => Vm.EditModeChanged -= OnEditModeChanged);
            Subscribe(() => Vm.CanSaveChanged -= OnCanSaveChanged);
            Subscribe(() => Vm.EquipmentsChanged -= MarkDirtyOrRefresh);

            _nameInput.onValueChanged.RemoveAllListeners();
            _nameInput.onValueChanged.AddListener(v => Vm.Name = v);

            _descInput.onValueChanged.RemoveAllListeners();
            _descInput.onValueChanged.AddListener(v => Vm.Description = v);

            _createButton.onClick.RemoveAllListeners();
            _createButton.onClick.AddListener(OnCreate);

            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(() => UIController.CloseScreen());

            _addEquipmentButton.onClick.RemoveAllListeners();
            _addEquipmentButton.onClick.AddListener(() => UIController.OpenScreen(ScreenType.CreateEquipment));

            OnEditModeChanged(Vm.IsEditMode);
            OnCanSaveChanged(Vm.CanSave);

            Refresh();
        }

        protected override void Refresh()
        {
            _isRefreshing = true;
            try
            {
                _nameInput.text = Vm.Name;
                _descInput.text = Vm.Description;
                
                List<EquipmentItem> items = _equipmentItemsGroup.Refresh(Vm.AllEquipments.Count);
                for (int i = 0; i < Vm.AllEquipments.Count; i++)
                {
                    Equipment eq = Vm.AllEquipments[i];
                    int quantity = Vm.GetQuantity(eq.Id);
                    items[i].Setup(eq, EquipmentItem.Mode.Edit, quantity, Vm.RemoveEquipment, Vm.UpdateEquipmentQuantity);
                }
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void OnCreate()
        {
            Vm.Save();
            UIController.CloseScreen();
        }

        private void OnEditModeChanged(bool editMode)
        {
            _header.text = editMode ? "Изменить упражнение" : "Создать упражнение";
            _createButtonText.text = editMode ? "Изменить" : "Создать";
        }

        private void OnCanSaveChanged(bool canSave) =>
            _createButton.interactable = canSave;
    }
}
