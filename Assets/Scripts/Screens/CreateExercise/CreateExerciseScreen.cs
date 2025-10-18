using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Views.Components;

namespace Screens.CreateExercise
{
    public class CreateExerciseScreen : ScreenWithViewModel<CreateExerciseViewModel>
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

        private readonly List<GameObject> _spawnedItems = new();

        public override async Task InitializeAsync(CreateExerciseViewModel viewModel, UiController uiController, object parameter = null)
        {
            await base.InitializeAsync(viewModel, uiController, parameter);

            Vm.EditModeChanged += OnEditModeChanged;
            Vm.CanSaveChanged += OnCanSaveChanged;
            Vm.EquipmentsChanged += MarkDirtyOrRefresh;

            Subscribe(() => Vm.EditModeChanged -= OnEditModeChanged);
            Subscribe(() => Vm.CanSaveChanged -= OnCanSaveChanged);
            Subscribe(() => Vm.EquipmentsChanged -= MarkDirtyOrRefresh);

            _nameInput.text = Vm.Name;
            _descInput.text = Vm.Description;

            _nameInput.onValueChanged.AddListener(v => Vm.Name = v);
            _descInput.onValueChanged.AddListener(v => Vm.Description = v);

            _createButton.onClick.AddListener(OnCreate);
            _backButton.onClick.AddListener(() => UIController.CloseScreen());
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
                foreach (Transform child in _equipmentListParent)
                    if (child.TryGetComponent(out EquipmentItem _))
                        SimplePool.Return(child.gameObject, _equipmentPrefab.gameObject);

                foreach (var eq in Vm.AllEquipments)
                {
                    int quantity = Vm.RequiredEquipment.Find(r => r.EquipmentId == eq.Id)?.Quantity ?? 0;
                    var go = SimplePool.Get(_equipmentPrefab.gameObject, _equipmentListParent);
                    var item = go.GetComponent<EquipmentItem>();
                    item.Setup(eq, EquipmentItem.Mode.Edit, quantity, Vm.RemoveEquipment, Vm.UpdateEquipmentQuantity);
                    _spawnedItems.Add(go);
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
