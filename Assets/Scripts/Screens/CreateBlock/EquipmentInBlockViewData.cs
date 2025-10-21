using Models;

namespace Screens.CreateBlock
{
    public class EquipmentInBlockViewData
    {
        public string Id;
        public string Name;
        public int Quantity;
        public bool NeedWeight;
        
        public int Weight;
        public WeightType WeightType;
    }
}