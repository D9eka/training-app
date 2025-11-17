using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class Exercise : IModel
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string Description { get; set; } = string.Empty;
        [field: SerializeField] public List<ExerciseEquipmentRef> RequiredEquipment { get; set; } = new();
        
        // TODO: Функция для упражнений с одной рукой: добавить тип повторения:
        // ПО (кол-во левой, кол-во правой; сделать указанное значение левой, сделать указанное значение правой),
        // ПЧ (один левой, один правой; пока не дойдет до значения)

        public Exercise()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Exercise(string name, string description = "") : this()
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? string.Empty;
        }

        public void AddOrUpdateEquipment(Equipment equipment, int quantity)
        {
            if (equipment == null) return;
            AddOrUpdateEquipment(equipment.Id, quantity);
        }

        public void AddOrUpdateEquipment(string equipmentId, int quantity)
        {
            if (string.IsNullOrEmpty(equipmentId)) return;

            RequiredEquipment.RemoveAll(r => r.EquipmentId == equipmentId);

            if (quantity > 0)
                RequiredEquipment.Add(new ExerciseEquipmentRef(equipmentId, quantity));
        }

        public void RemoveEquipment(string equipmentId)
        {
            RequiredEquipment.RemoveAll(r => r.EquipmentId == equipmentId);
        }
    }
}
