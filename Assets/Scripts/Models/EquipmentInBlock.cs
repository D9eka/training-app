using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class EquipmentInBlock
    {
        [field: SerializeField] public string Id { get; set; }
        [field: SerializeField] public ExerciseEquipmentRef Equipment { get; set; }
        [field: SerializeField] public float Weight { get; set; }
        [field: SerializeField] public WeightType WeightType { get; set; }

        public EquipmentInBlock(ExerciseEquipmentRef equipment, float weight = 0, WeightType weightType = WeightType.Kg)
        {
            Id = Guid.NewGuid().ToString();
            Equipment = equipment;
            Weight = weight;
            WeightType = weightType;
        }
    }
}