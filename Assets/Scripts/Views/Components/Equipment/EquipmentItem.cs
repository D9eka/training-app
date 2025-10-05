using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class EquipmentItem : MonoBehaviour
{
    [SerializeField] private Button _nameButton;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Button _addQtyButton;
    [SerializeField] private Button _removeQtyButton;
    [SerializeField] private TMP_Text _quantityText;
    [SerializeField] private TMP_InputField _weightInput;
    [SerializeField] private Button _deleteButton;

    public enum Mode { View, Create, TrainingView, CreateTraining }

    private Equipment _equipment;
    private int _quantity;
    private bool _isQuantityVisible => _quantity > 0;
    private Action<Equipment> _onDelete;
    private Action<Equipment, int> _onQuantityChanged;

    public void Setup(Equipment equipment, Mode mode, int quantity, Action<Equipment> onDelete, Action<Equipment, int> onQuantityChanged = null)
    {
        _equipment = equipment;
        _quantity = quantity;
        _onDelete = onDelete;
        _onQuantityChanged = onQuantityChanged;

        _nameText.text = equipment.Name;
        _quantityText.text = quantity.ToString();

        bool showWeight = equipment.HasWeight && mode == Mode.CreateTraining;

        bool ww = _isQuantityVisible && mode == Mode.Create;
        _removeQtyButton.gameObject.SetActive(_isQuantityVisible && mode == Mode.Create);
        _quantityText.gameObject.SetActive(_isQuantityVisible);
        _addQtyButton.gameObject.SetActive(_isQuantityVisible && mode == Mode.Create);
        
        _weightInput.gameObject.SetActive(showWeight);
        _weightInput.interactable = showWeight;
        
        _deleteButton.gameObject.SetActive(mode == Mode.Create);
        
        _nameButton.onClick.AddListener(OnNameClicked);
        if (_addQtyButton.gameObject.activeSelf) 
            _addQtyButton.onClick.AddListener(OnAddQty);
        if (_removeQtyButton.gameObject.activeSelf)
            _removeQtyButton.onClick.AddListener(OnRemoveQty);
        if (_deleteButton.gameObject.activeSelf)
            _deleteButton.onClick.AddListener(OnDelete);
    }

    private void OnNameClicked()
    {
        if (_quantity == 0)
        {
            _quantity = 1;
            UpdateQuantityUI();
            _onQuantityChanged?.Invoke(_equipment, _quantity);
        }
    }

    private void OnAddQty()
    {
        _quantity++;
        UpdateQuantityUI();
        _onQuantityChanged?.Invoke(_equipment, _quantity);
    }

    private void OnRemoveQty()
    {
        if (_quantity > 0) _quantity--;
        UpdateQuantityUI();
        _onQuantityChanged?.Invoke(_equipment, _quantity);
    }

    private void OnDelete()
    {
        _onDelete?.Invoke(_equipment);
        Destroy(gameObject);
    }

    private void UpdateQuantityUI()
    {
        _quantityText.text = _quantity.ToString();
        _removeQtyButton.gameObject.SetActive(_isQuantityVisible);
        _quantityText.gameObject.SetActive(_isQuantityVisible);
        _addQtyButton.gameObject.SetActive(_isQuantityVisible || _equipment.HasQuantity);
    }

    private void OnDestroy()
    {
        _nameButton.onClick.RemoveAllListeners();
        if (_addQtyButton != null) _addQtyButton.onClick.RemoveAllListeners();
        if (_removeQtyButton != null) _removeQtyButton.onClick.RemoveAllListeners();
        if (_deleteButton != null) _deleteButton.onClick.RemoveAllListeners();
    }
}