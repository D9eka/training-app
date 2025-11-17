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
        [field: SerializeField] public int ApproachesSeconds { get; set; }
        [field: SerializeField] public int Sets { get; set; }
        [field: SerializeField] public int RestAfterApproachSeconds { get; set; }
        [field: SerializeField] public int RestAfterSetSeconds { get; set; }
        [field: SerializeField] public int RestAfterBlockSeconds { get; set; }

        public TimeSpan ApproachesTimeSpan => TimeSpan.FromSeconds(ApproachesSeconds);
        public TimeSpan RestAfterApproachTimeSpan => TimeSpan.FromSeconds(RestAfterApproachSeconds);
        public TimeSpan RestAfterSetTimeSpan => TimeSpan.FromSeconds(RestAfterSetSeconds);
        public TimeSpan RestAfterBlockTimeSpan => TimeSpan.FromSeconds(RestAfterBlockSeconds);

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
            ApproachesSeconds = (int)approachesTimeSpan.TotalSeconds;
            Sets = sets;
            RestAfterApproachSeconds = (int)restAfterApproachTimeSpan.TotalSeconds;
            RestAfterSetSeconds = (int)restAfterSetTimeSpan.TotalSeconds;
            RestAfterBlockSeconds = (int)restAfterBlockTimeSpan.TotalSeconds;
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
