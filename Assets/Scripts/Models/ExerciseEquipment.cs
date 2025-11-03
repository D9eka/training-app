using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class ExerciseEquipment
    {
        [field: SerializeField] public Equipment Equipment { get; set; }
        [field: SerializeField] public int Quantity { get; set; }

        public ExerciseEquipment(Equipment equipment, int quantity)
        {
            Equipment = equipment;
            Quantity = quantity;
        }
    }
}