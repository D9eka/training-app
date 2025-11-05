namespace Screens.Factories.Parameters
{
    public class TrainingIdParameter : IScreenParameter
    {
        public string TrainingId { get; private set; }
        
        public TrainingIdParameter(string trainingId)
        {
            TrainingId = trainingId;
        }
    }
}