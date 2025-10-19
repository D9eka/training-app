using System;
using System.Collections.Generic;

namespace Screens.CreateTraining
{
    public class TrainingBlockViewData
    {
        public string Id;
        public List<ExerciseInBlockViewData> ExercisesInBlockViewData;
        public int Approaches;
        public TimeSpan ApproachesTimeSpan;
        public TimeSpan RestAfterApproachTimeSpan;
        public int Sets;
        public TimeSpan RestAfterSetsTimeSpan;
        public TimeSpan RestAfterBlockTimeSpan;
    }
}