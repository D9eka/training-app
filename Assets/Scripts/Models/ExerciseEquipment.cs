using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class ExerciseEquipment
    {
        [SerializeField] private Equipment _equipment;
        [SerializeField] private int _quantity;

        public Equipment Equipment
        {
            get => _equipment;
            set => _equipment = value;
        }

        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }

        public ExerciseEquipment(Equipment equipment, int quantity)
        {
            _equipment = equipment;
            _quantity = quantity;
        }
    }
}