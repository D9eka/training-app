using System;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Components
{
    public class EquipmentItem : MonoBehaviour
    {
        public enum Mode { View, Edit, WorkoutView, WorkoutEdit }

        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _quantityText;
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private Button _deleteButton;
        [SerializeField] private TMP_InputField _weightInput;

        private Equipment _equipment;
        private Action<Equipment> _onDelete;
        private Action<Equipment, int> _onQuantityChanged;
        private int _quantity;
        
        public void Setup(Equipment eq, Mode mode, int quantity = 0,
            Action<Equipment> onDelete = null,
            Action<Equipment, int> onQuantityChanged = null)
        {
            _equipment = eq ?? throw new ArgumentNullException(nameof(eq));
            _onDelete = onDelete;
            _onQuantityChanged = onQuantityChanged;
            _quantity = Math.Clamp(quantity, 0, 100);

            _nameText.text = eq.Name;
            UpdateQuantityText();

            _addButton.onClick.RemoveAllListeners();
            _removeButton.onClick.RemoveAllListeners();
            _deleteButton.onClick.RemoveAllListeners();

            _addButton.onClick.AddListener(() => ChangeQuantity(1));
            _removeButton.onClick.AddListener(() => ChangeQuantity(-1));
            _deleteButton.onClick.AddListener(() => _onDelete?.Invoke(_equipment));

            ConfigureMode(mode);
        }

        private void ConfigureMode(Mode mode)
        {
            bool canEdit = mode == Mode.Edit || mode == Mode.WorkoutEdit;
            bool showWeight = mode == Mode.WorkoutView || mode == Mode.WorkoutEdit;
            bool showDelete = mode == Mode.Edit;

            _addButton.gameObject.SetActive(canEdit && _equipment.HasQuantity);
            _removeButton.gameObject.SetActive(canEdit);
            _deleteButton.gameObject.SetActive(showDelete);
            _weightInput.gameObject.SetActive(showWeight);
            _quantityText.gameObject.SetActive(true);
        }

        private void ChangeQuantity(int delta)
        {
            _quantity = Math.Clamp(_quantity + delta, 0, 100);
            UpdateQuantityText();
            _onQuantityChanged?.Invoke(_equipment, _quantity);
        }

        private void UpdateQuantityText() =>
            _quantityText.text = _quantity > 0 ? $"x{_quantity}" : "-";
    }
}