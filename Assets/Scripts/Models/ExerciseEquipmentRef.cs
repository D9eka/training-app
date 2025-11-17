using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class ExerciseEquipmentRef
    {
        [field: SerializeField] public string EquipmentId { get; private set; }
        [field: SerializeField] public int Quantity { get; set; }

        public ExerciseEquipmentRef() { }

        public ExerciseEquipmentRef(Equipment equipment, int quantity)
            : this(equipment?.Id, quantity)
        {
        }

        public ExerciseEquipmentRef(string equipmentId, int quantity)
        {
            EquipmentId = equipmentId;
            Quantity = Math.Max(0, quantity);
        }

        public void SetEquipment(string equipmentId)
        {
            EquipmentId = equipmentId;
        }

        public void SetQuantity(int quantity)
        {
            Quantity = Math.Max(0, quantity);
        }
    }
}
