namespace Models
{
    public class EquipmentInBlock
    {
        public Equipment Equipment;
        public float Weight;
        public WeightType WeightType;

        public EquipmentInBlock(Equipment equipment, float weight=0, WeightType weightType=WeightType.Kg)
        {
            Equipment = equipment;
            Weight = weight;
            WeightType = weightType;
        }
    }
}