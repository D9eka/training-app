using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class ExerciseInBlock
    {
        public Exercise Exercise;
        public List<EquipmentInBlock>  EquipmentWeights;
        public int Repetitions;
        public float DurationSeconds;

        public ExerciseInBlock(Exercise exercise, List<EquipmentInBlock> equipmentWeights, float weight, WeightType weightType, int repetitions, float durationSeconds)
        {
            Exercise = exercise;
            EquipmentWeights = equipmentWeights;
            Repetitions = repetitions;
            DurationSeconds = durationSeconds;
        }
    }
}