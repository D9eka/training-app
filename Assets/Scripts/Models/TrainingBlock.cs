using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class TrainingBlock : IModel
    {
        [field: SerializeField] public string TrainingId { get; private set; }
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public List<ExerciseInBlock> Exercises { get; set; } = new();
        [field: SerializeField] public int Approaches { get; set; }
        [field: SerializeField] public TimeSpan ApproachesTimeSpan { get; set; }
        [field: SerializeField] public int Sets { get; set; }
        [field: SerializeField] public TimeSpan RestAfterApproachTimeSpan { get; set; }
        [field: SerializeField] public TimeSpan RestAfterSetTimeSpan { get; set; }
        [field: SerializeField] public TimeSpan RestAfterBlockTimeSpan { get; set; }

        public TrainingBlock(string trainingId)
        {
            TrainingId = trainingId;
            Id = Guid.NewGuid().ToString();
        }

        public TrainingBlock(string trainingId, List<ExerciseInBlock> exercises, int approaches, TimeSpan approachesTimeSpan,
            int sets, TimeSpan restAfterApproachTimeSpan, TimeSpan restAfterSetTimeSpan,
            TimeSpan restAfterBlockTimeSpan) : this(trainingId)
        {
            Exercises = exercises ?? new List<ExerciseInBlock>();
            Approaches = approaches;
            ApproachesTimeSpan = approachesTimeSpan;
            Sets = sets;
            RestAfterApproachTimeSpan = restAfterApproachTimeSpan;
            RestAfterSetTimeSpan = restAfterSetTimeSpan;
            RestAfterBlockTimeSpan = restAfterBlockTimeSpan;
        }

        public void AddExercise(ExerciseInBlock exercise, int exerciseIndex = -1)
        {
            Exercises ??= new List<ExerciseInBlock>();
            if (exerciseIndex < 0) 
                Exercises.Add(exercise);
            else
                Exercises.Insert(exerciseIndex, exercise);
        }

        public void RemoveExercise(string exerciseId)
        {
            Exercises?.RemoveAll(e => e.ExerciseId == exerciseId);
        }
    }
}