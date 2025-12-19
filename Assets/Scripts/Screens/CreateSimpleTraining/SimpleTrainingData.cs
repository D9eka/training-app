namespace Screens.CreateSimpleTraining
{
    public class SimpleTrainingData
    {
        public SimpleTrainingData(int prepTimeSeconds, int approachDurationSeconds, int restAfterApproachSeconds, 
            int approaches, int sets, int restAfterSetSeconds)
        {
            ApproachDurationSeconds = approachDurationSeconds;
            PrepTimeSeconds = prepTimeSeconds;
            RestAfterApproachSeconds = restAfterApproachSeconds;
            Approaches = approaches;
            Sets = sets;
            RestAfterSetSeconds = restAfterSetSeconds;
        }

        public int PrepTimeSeconds { get; private set; }
        public int ApproachDurationSeconds { get; private set; }
        public int RestAfterApproachSeconds { get; private set; }
        public int Approaches { get; private set; }
        public int Sets { get; private set; }
        public int RestAfterSetSeconds { get; private set; }
    }
}