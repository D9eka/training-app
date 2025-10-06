using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using TMPro;
using UnityEngine;
using Views.Components.Equipment;

namespace Screens.ViewExercise
{
    public class ViewExerciseScreen : ScreenBase
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descText;
        [SerializeField] private Transform _equipmentListParent;
        [SerializeField] private EquipmentItem _equipmentItemPrefab;

        private ViewExerciseViewModel _vm;
        private List<EquipmentItem> _spawnedItems = new();

        public override async Task InitializeAsync(object parameter = null)
        {
            _vm = new ViewExerciseViewModel();
            _vm.ExerciseChanged += RefreshUI;

            if (parameter is Exercise ex)
            {
                _vm.SetExercise(ex);
            }

            await Task.CompletedTask;
        }

        private void OnEnable()
        {
            if (_vm == null) return;
            _vm.ExerciseChanged += RefreshUI;
            RefreshUI();
        }

        private void OnDisable()
        {
            _vm.ExerciseChanged -= RefreshUI;
        }

        private void RefreshUI()
        {
            _nameText.text = _vm.CurrentExercise.Name;
            _descText.text = $"Описание: {_vm.CurrentExercise.Description}";

            foreach (EquipmentItem item in _spawnedItems)
            {
                Destroy(item.gameObject);
            }
            _spawnedItems.Clear();

            foreach (ExerciseEquipment reqEq in _vm.CurrentExercise.RequiredEquipment)
            {
                EquipmentItem item = Instantiate(_equipmentItemPrefab, _equipmentListParent);
                item.Setup(reqEq.Equipment, EquipmentItem.Mode.View, reqEq.Quantity, null);
                _spawnedItems.Add(item);
            }
        }

        protected override void OnEditClicked()
        {
            ServiceLocator.Instance.SetScreenParameter(_vm.CurrentExercise.Id);
            UiController.OpenScreen(ServiceLocator.Instance.EditExerciseScreen.gameObject, true, false);
        }
    }
}