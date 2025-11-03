using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class ExerciseEquipmentRef
    {
        [field: SerializeField] public Equipment Equipment { get; private set; }
        [field: SerializeField] public int Quantity { get; set; }

        public ExerciseEquipmentRef() { }

        public ExerciseEquipmentRef(Equipment equipment, int quantity)
        {
            Equipment = equipment;
            Quantity = Math.Max(0, quantity);
        }

        public void SetQuantity(int quantity)
        {
            Quantity = Math.Max(0, quantity);
        }
    }
}