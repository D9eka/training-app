using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class ExerciseEquipmentRef
    {
        [SerializeField] private string _equipmentId;
        [SerializeField] private int _quantity;

        public string EquipmentId
        {
            get => _equipmentId;
            private set => _equipmentId = value;
        }

        public int Quantity
        {
            get => _quantity;
            set => _quantity = Math.Max(0, value);
        }

        public ExerciseEquipmentRef() { }

        public ExerciseEquipmentRef(string equipmentId, int quantity)
        {
            if (string.IsNullOrEmpty(equipmentId)) throw new ArgumentException("EquipmentId cannot be empty", nameof(equipmentId));
            _equipmentId = equipmentId;
            _quantity = Math.Max(0, quantity);
        }
    }
}