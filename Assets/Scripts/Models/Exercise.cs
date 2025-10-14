using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class Exercise
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private List<ExerciseEquipmentRef> _requiredEquipment;

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

        public string Description
        {
            get => _description;
            set => _description = value ?? string.Empty;
        }

        public IReadOnlyList<ExerciseEquipmentRef> RequiredEquipment => _requiredEquipment.AsReadOnly();

        public Exercise()
        {
            _requiredEquipment = new List<ExerciseEquipmentRef>();
        }

        public Exercise(string name, string description = "")
        {
            _id = Guid.NewGuid().ToString();
            _name = name;
            _description = description ?? string.Empty;
            _requiredEquipment = new List<ExerciseEquipmentRef>();
        }

        public void AddOrUpdateEquipment(string equipmentId, int quantity)
        {
            if (string.IsNullOrEmpty(equipmentId)) return;
            if (quantity <= 0)
            {
                _requiredEquipment.RemoveAll(r => r.EquipmentId == equipmentId);
                return;
            }
            ExerciseEquipmentRef existing = _requiredEquipment.Find(r => r.EquipmentId == equipmentId);
            if (existing != null)
                existing.Quantity = quantity;
            else
                _requiredEquipment.Add(new ExerciseEquipmentRef(equipmentId, quantity));
        }

        public void RemoveEquipment(string equipmentId)
        {
            _requiredEquipment.RemoveAll(r => r.EquipmentId == equipmentId);
        }

        public int GetQuantity(string equipmentId)
        {
            if (string.IsNullOrEmpty(equipmentId)) return 0;
            ExerciseEquipmentRef r = _requiredEquipment.Find(x => x.EquipmentId == equipmentId);
            return r?.Quantity ?? 0;
        }
    }
}