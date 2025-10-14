namespace Models
{
    public class EquipmentInBlock
    {
        public ExerciseEquipmentRef Equipment;
        public float Weight;
        public WeightType WeightType;

        public EquipmentInBlock(ExerciseEquipmentRef equipment, float weight=0, WeightType weightType=WeightType.Kg)
        {
            Equipment = equipment;
            Weight = weight;
            WeightType = weightType;
        }
    }
}