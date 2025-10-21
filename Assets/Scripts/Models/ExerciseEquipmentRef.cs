using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class ExerciseEquipmentRef
    {
        [SerializeField] private Equipment _equipment;
        [SerializeField] private int _quantity;

        public Equipment Equipment
        {
            get => _equipment;
            private set => _equipment = value;
        }

        public int Quantity
        {
            get => _quantity;
            set => _quantity = Math.Max(0, value);
        }

        public ExerciseEquipmentRef() { }

        public ExerciseEquipmentRef(Equipment equipment, int quantity)
        {
            _equipment = equipment;
            _quantity = quantity;
        }
    }
}