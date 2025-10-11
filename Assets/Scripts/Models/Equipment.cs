using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class Equipment
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private bool _hasQuantity;
        [SerializeField] private bool _hasWeight;

        public string Id
        {
            get => _id;
            private set => _id = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool HasQuantity
        {
            get => _hasQuantity;
            set => _hasQuantity = value;
        }

        public bool HasWeight
        {
            get => _hasWeight;
            set => _hasWeight = value;
        }

        public Equipment() { }

        public Equipment(string name, bool hasQuantity = false, bool hasWeight = false)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty", nameof(name));
            _id = Guid.NewGuid().ToString();
            _name = name;
            _hasQuantity = hasQuantity;
            _hasWeight = hasWeight;
        }
    }
}