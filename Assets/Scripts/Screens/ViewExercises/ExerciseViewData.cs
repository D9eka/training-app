using System.Collections.Generic;

namespace Screens.ViewExercises
{
    public class ExerciseViewData
    {
        public string Id;
        public string Name;
        public string Description;
        public List<(string EquipmentName, int Quantity)> Equipments;
    }
}