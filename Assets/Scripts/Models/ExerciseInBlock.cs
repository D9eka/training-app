using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class ExerciseInBlock
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string ExerciseId { get; private set; }
        [field: SerializeField] public List<EquipmentInBlock> EquipmentWeights { get; set; }
        [field: SerializeField] public int Repetitions { get; set; }
        [field: SerializeField] public TimeSpan DurationTimeSpan { get; set; }

        public ExerciseInBlock(Exercise exercise)
        {
            Id = Guid.NewGuid().ToString();
            ExerciseId = exercise.Id;
            EquipmentWeights = new List<EquipmentInBlock>();
            foreach (ExerciseEquipmentRef exerciseEquipmentRef in exercise.RequiredEquipment)
            {
                if (exerciseEquipmentRef.Equipment.HasWeight)
                {
                    EquipmentWeights.Add(new EquipmentInBlock(exerciseEquipmentRef));
                }
            }
            Repetitions = 0;
            DurationTimeSpan = TimeSpan.Zero;
        }
    }
}