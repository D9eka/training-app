using System.Collections.Generic;

namespace Screens.CreateBlock
{
    public class ExerciseInBlockViewData
    {
        public string Id;
        public string Name;
        public List<EquipmentInBlockViewData> Equipments;
        public int Repetitions;
        public int DurationSeconds;
    }
}