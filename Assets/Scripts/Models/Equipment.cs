using System;

namespace Models
{
    [Serializable]
    public class Equipment
    {
        public string Id;
        public string Name;
        public bool HasQuantity;
        public bool HasWeight;

        public Equipment(string name, bool hasQuantity, bool hasWeight)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            HasQuantity = hasQuantity;
            HasWeight = hasWeight;
        }
    }
}