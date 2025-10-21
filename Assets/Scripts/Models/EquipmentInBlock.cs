using System;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class EquipmentInBlock
    {
        [SerializeField] private string _id;
        [SerializeField] private ExerciseEquipmentRef _equipment;
        [SerializeField] private float _weight;
        [SerializeField] private WeightType _weightType;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public ExerciseEquipmentRef Equipment
        {
            get => _equipment;
            set => _equipment = value;
        }

        public float Weight
        {
            get => _weight;
            set => _weight = value;
        }

        public WeightType WeightType
        {
            get => _weightType;
            set => _weightType = value;
        }

        public EquipmentInBlock(ExerciseEquipmentRef equipment, float weight = 0, WeightType weightType = WeightType.Kg)
        {
            _id = Guid.NewGuid().ToString();
            _equipment = equipment;
            _weight = weight;
            _weightType = weightType;
        }
    }
}