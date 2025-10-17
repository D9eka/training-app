using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class TrainingBlock
    {
        public List<ExerciseInBlock> Exercises = new();
        public int Approaches;
        public int ApproachesTimeSeconds;
        public int Sets;
        public float RestAfterApproachSeconds;
        public float RestAfterSetSeconds;
        public float RestAfterBlockSeconds;

        public TrainingBlock(int approaches, int approachesTimeSeconds, int sets, 
            float restAfterApproachSeconds, float restAfterSetSeconds, float restAfterBlockSeconds)
        {
            Approaches = approaches;
            ApproachesTimeSeconds = approachesTimeSeconds;
            Sets = sets;
            RestAfterApproachSeconds = restAfterApproachSeconds;
            RestAfterSetSeconds = restAfterSetSeconds;
            RestAfterBlockSeconds = restAfterBlockSeconds;
        }

        public void RemoveExercise(string exerciseId)
        {
            Exercises.RemoveAll(r => r.Exercise.Id == exerciseId);
        }
    }
}