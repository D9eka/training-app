using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class ExerciseInBlock
    {
        [SerializeField] private string _id;
        [SerializeField] private Exercise _exercise;
        [SerializeField] private List<EquipmentInBlock> _equipmentWeights;
        [SerializeField] private int _repetitions;
        [SerializeField] private TimeSpan _durationTimeSpan;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public Exercise Exercise
        {
            get => _exercise;
            set => _exercise = value;
        }

        public List<EquipmentInBlock> EquipmentWeights
        {
            get => _equipmentWeights;
            set => _equipmentWeights = value;
        }

        public int Repetitions
        {
            get => _repetitions;
            set => _repetitions = value;
        }

        public TimeSpan DurationTimeSpan
        {
            get => _durationTimeSpan;
            set => _durationTimeSpan = value;
        }

        public ExerciseInBlock(Exercise exercise)
        {
            _id = Guid.NewGuid().ToString();
            _exercise = exercise;
            _equipmentWeights = new List<EquipmentInBlock>();
            foreach (ExerciseEquipmentRef exerciseEquipmentRef in exercise.RequiredEquipment)
            {
                if (exerciseEquipmentRef.Equipment.HasWeight)
                {
                    _equipmentWeights.Add(new EquipmentInBlock(exerciseEquipmentRef));
                }
            }
            _repetitions = 0;
            _durationTimeSpan = TimeSpan.Zero;
        }
    }
}