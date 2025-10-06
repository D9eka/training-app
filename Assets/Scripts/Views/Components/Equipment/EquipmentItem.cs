using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Components.Equipment
{
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

        private Models.Equipment _equipment;
        private int _quantity;
        private bool _isQuantityVisible => _quantity > 0;
        private Action<Models.Equipment> _onDelete;
        private Action<Models.Equipment, int> _onQuantityChanged;

        public void Setup(Models.Equipment equipment, Mode mode, int quantity, Action<Models.Equipment> onDelete, Action<Models.Equipment, int> onQuantityChanged = null)
        {
            _equipment = equipment;
            _quantity = quantity;
            _onDelete = onDelete;
            _onQuantityChanged = onQuantityChanged;

            _nameText.text = equipment.Name;
            _quantityText.text = $"x{quantity.ToString()}";
        
            _nameButton.onClick.AddListener(OnNameClicked);
            _addQtyButton.onClick.AddListener(OnAddQty);
            _removeQtyButton.onClick.AddListener(OnRemoveQty);
            _deleteButton.onClick.AddListener(OnDelete);
            
            _removeQtyButton.gameObject.SetActive(_isQuantityVisible && mode == Mode.Create);
            _quantityText.gameObject.SetActive(_isQuantityVisible);
            _addQtyButton.gameObject.SetActive(_isQuantityVisible && _equipment.HasQuantity && mode == Mode.Create);
        
            bool showWeight = equipment.HasWeight && mode == Mode.CreateTraining;
            _weightInput.gameObject.SetActive(showWeight);
            _weightInput.interactable = showWeight;
        
            _deleteButton.gameObject.SetActive(mode == Mode.Create);
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
            _addQtyButton.gameObject.SetActive(_isQuantityVisible && _equipment.HasQuantity);
        }
    }
}