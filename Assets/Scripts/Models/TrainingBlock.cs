using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class TrainingBlock
    {
        public string Id { get; private set; }
        public List<ExerciseInBlock> Exercises;
        public int Approaches;
        public TimeSpan ApproachesTimeSpan;
        public int Sets;
        public TimeSpan RestAfterApproachTimeSpan;
        public TimeSpan RestAfterSetTimeSpan;
        public TimeSpan RestAfterBlockTimeSpan;

        public TrainingBlock()
        {
            Id = Guid.NewGuid().ToString();
            Exercises =  new List<ExerciseInBlock>();
        }

    public TrainingBlock(List<ExerciseInBlock> Exercises, int approaches, TimeSpan approachesTimeSpan, int sets, 
        TimeSpan restAfterApproachTimeSpan, TimeSpan restAfterSetTimeSpan, TimeSpan restAfterBlockTimeSpan) : this()
        {
            Approaches = approaches;
            ApproachesTimeSpan = approachesTimeSpan;
            Sets = sets;
            RestAfterApproachTimeSpan = restAfterApproachTimeSpan;
            RestAfterSetTimeSpan = restAfterSetTimeSpan;
            RestAfterBlockTimeSpan = restAfterBlockTimeSpan;
        }

        public void RemoveExercise(string exerciseId)
        {
            Exercises.RemoveAll(r => r.Exercise.Id == exerciseId);
        }
    }
}