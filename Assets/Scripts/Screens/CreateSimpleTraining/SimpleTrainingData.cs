namespace Screens.Timer
{
    public class SimpleTrainingData
    {
        public SimpleTrainingData(int exerciseDurationSeconds, int prepTimeSeconds, int approaches, int sets, 
            int restAfterApproachSeconds, int restAfterSetSeconds)
        {
            ExerciseDurationSeconds = exerciseDurationSeconds;
            PrepTimeSeconds = prepTimeSeconds;
            Approaches = approaches;
            Sets = sets;
            RestAfterApproachSeconds = restAfterApproachSeconds;
            RestAfterSetSeconds = restAfterSetSeconds;
        }

        public int ExerciseDurationSeconds { get; private set; }
        public int PrepTimeSeconds { get; private set; }
        public int Approaches { get; private set; }
        public int Sets { get; private set; }
        public int RestAfterApproachSeconds { get; private set; }
        public int RestAfterSetSeconds { get; private set; }
    }
}