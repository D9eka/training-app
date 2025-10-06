using System;

namespace Models
{
    [Serializable]
    public class ExerciseEquipment
    {
        public Equipment Equipment;
        public int Quantity;

        public ExerciseEquipment(Equipment equipment, int quantity)
        {
            Equipment = equipment;
            Quantity = quantity;
        }
    }
}