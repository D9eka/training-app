using UnityEngine.UI;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ViewExerciseScreen : ScreenBase
{
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _descText;
    [SerializeField] private Transform _equipmentListParent;
    [SerializeField] private EquipmentItem _equipmentItemPrefab;

    private ViewExerciseViewModel _vm;
    private List<EquipmentItem> _spawnedItems = new List<EquipmentItem>();

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

    private void OnDestroy()
    {
        _vm.ExerciseChanged -= RefreshUI;
    }

    private void RefreshUI()
    {
        _nameText.text = _vm.CurrentExercise.Name;
        _descText.text = _vm.CurrentExercise.Description;

        foreach (var item in _spawnedItems)
        {
            Destroy(item.gameObject);
        }
        _spawnedItems.Clear();

        foreach (var reqEq in _vm.CurrentExercise.RequiredEquipment)
        {
            var item = Instantiate(_equipmentItemPrefab, _equipmentListParent);
            item.Setup(reqEq.Equipment, EquipmentItem.Mode.View, reqEq.Quantity, null);
            _spawnedItems.Add(item);
        }
    }
}