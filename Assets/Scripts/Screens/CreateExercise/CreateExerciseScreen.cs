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
    public class CreateExerciseScreen : ReactiveScreen
    {
        [SerializeField] private TMP_Text _header;
        
        [Header("Exercise Fields")]
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TMP_InputField _descInput;

        [Header("Equipments")]
        [SerializeField] private Transform _equipmentListParent;
        [SerializeField] private EquipmentItem _equipmentPrefab;

        [Header("Buttons")]
        [SerializeField] private Button _createButton;
        [SerializeField] private TMP_Text _createButtonText;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _addEquipmentButton;

        private CreateExerciseViewModel _vm;
        private readonly List<GameObject> _spawnedItems = new();

        private UiController _uiController;

        private void Awake()
        {
            _uiController = DiContainer.Instance.Resolve<UiController>();
        }

        public override async Task InitializeAsync(object parameter = null)
        {
            var factory = DiContainer.Instance.Resolve<ViewModelFactory>();
            _vm = factory.Create<CreateExerciseViewModel>(parameter);

            Subscribe(() => _vm.EditModeChanged -= OnEditModeChanged);
            Subscribe(() => _vm.CanSaveChanged -= OnCanSaveChanged);
            Subscribe(() => _vm.EquipmentsChanged -= MarkDirtyOrRefresh);

            _vm.EditModeChanged += OnEditModeChanged;
            _vm.CanSaveChanged += OnCanSaveChanged;
            _vm.EquipmentsChanged += MarkDirtyOrRefresh;

            _nameInput.text = _vm.Name;
            _descInput.text = _vm.Description;
            _nameInput.onValueChanged.AddListener(v => _vm.Name = v);
            _descInput.onValueChanged.AddListener(v => _vm.Description = v);

            _createButton.onClick.AddListener(OnCreate);
            _backButton.onClick.AddListener(() => _uiController.CloseScreen());
            _addEquipmentButton.onClick.AddListener(() => _uiController.OpenScreen(ScreenType.CreateEquipment));

            RefreshEquipments();
            OnCanSaveChanged(_vm.CanSave);
            await base.InitializeAsync(parameter);
        }

        protected override void Refresh()
        {
            _isRefreshing =  true;
            try
            {
                RefreshEquipments();
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void RefreshEquipments()
        {
            if (_initialized && !isDirty) return;
            
            foreach (Transform child in _equipmentListParent)
                if (child.TryGetComponent(out EquipmentItem _))
                    SimplePool.Return(child.gameObject, _equipmentPrefab.gameObject);

            foreach (var eq in _vm.AllEquipments)
            {
                int quantity = _vm.RequiredEquipment.Find(r => r.EquipmentId == eq.Id)?.Quantity ?? 0;
                var go = SimplePool.Get(_equipmentPrefab.gameObject, _equipmentListParent);
                var item = go.GetComponent<EquipmentItem>();
                item.Setup(eq, EquipmentItem.Mode.Edit, quantity, _vm.RemoveEquipment, _vm.UpdateEquipmentQuantity);
                _spawnedItems.Add(go);
            }
        }

        private void OnCreate()
        {
            _vm.Save();
            _uiController.CloseScreen();
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
