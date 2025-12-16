using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class Equipment : IModel
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public bool HasQuantity { get; set; }
        [field: SerializeField] public bool HasWeight { get; set; }

        public Equipment(string name, bool hasQuantity = false, bool hasWeight = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Weight cannot be empty", nameof(name));

            Id = Guid.NewGuid().ToString();
            Name = name;
            HasQuantity = hasQuantity;
            HasWeight = hasWeight;
        }

        public void SetName(string name)
        {
            Name = name ?? throw new ArgumentException("Weight cannot be empty", nameof(name));
        }
    }
}