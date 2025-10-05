using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CreateEquipmentScreen : ScreenBase
{
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private Toggle _quantityToggle;
    [SerializeField] private Toggle _weightToggle;

    private CreateEquipmentViewModel _vm;

    public override async Task InitializeAsync(object parameter = null)
    {
        _vm = new CreateEquipmentViewModel(ServiceLocator.Instance.DataService);

        _nameInput.text = _vm.Name;
        _quantityToggle.isOn = _vm.HasQuantity;
        _weightToggle.isOn = _vm.HasWeight;

        _nameInput.onValueChanged.AddListener(value => 
        {
            _vm.Name = value;
            UpdateSaveButton(!string.IsNullOrEmpty(value));
        });
        _quantityToggle.onValueChanged.AddListener(value => _vm.HasQuantity = value);
        _weightToggle.onValueChanged.AddListener(value => _vm.HasWeight = value);

        UpdateSaveButton(!string.IsNullOrEmpty(_vm.Name));

        await Task.CompletedTask;
    }

    private void UpdateSaveButton(bool canSave)
    {
        _saveButton.interactable = canSave;
    }

    protected override void OnSaveClicked()
    {
        _vm.Save();
        UiController.CloseScreen();
    }
}