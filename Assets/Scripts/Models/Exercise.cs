using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class Exercise : IModel
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private List<ExerciseEquipmentRef> _requiredEquipment;

        // TODO: Функция для упражнений с одной рукой: добавить тип повторения:
        // ПО (кол-во левой, кол-во правой; сделать указанное значение левой, сделать указанное значение правой),
        // ПЧ (один левой, один правой; пока не дойдет до значения)
        
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

        public List<ExerciseEquipmentRef> RequiredEquipment => _requiredEquipment;

        public Exercise()
        {
            _id = Guid.NewGuid().ToString();
            _requiredEquipment = new List<ExerciseEquipmentRef>();
        }

        public Exercise(string name, string description = "") : this()
        {
            _name = name;
            _description = description ?? string.Empty;
        }

        public void AddOrUpdateEquipment(Equipment equipment, int quantity)
        {
            if (equipment == null) return;
            if (quantity <= 0)
            {
                _requiredEquipment.RemoveAll(r => r.Equipment.Id == equipment.Id);
                return;
            }
            ExerciseEquipmentRef existing = _requiredEquipment.Find(r => r.Equipment.Id == equipment.Id);
            if (existing != null)
                existing.Quantity = quantity;
            else
                _requiredEquipment.Add(new ExerciseEquipmentRef(equipment, quantity));
        }

        public void RemoveEquipment(string equipmentId)
        {
            _requiredEquipment.RemoveAll(r => r.Equipment.Id == equipmentId);
        }
    }
}