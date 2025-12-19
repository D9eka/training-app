using Screens.CreateSimpleTraining;

namespace Screens.Factories.Parameters
{
    public class TimerParameter : TrainingIdParameter
    {
        public readonly SimpleTrainingData SimpleTrainingData;
        public readonly bool HasTrainingId;
        public readonly bool HaveSimpleTrainingData;

        public TimerParameter(SimpleTrainingData simpleTrainingData) : this(null, simpleTrainingData)
        {
        }
        
        public TimerParameter(string trainingId, SimpleTrainingData simpleTrainingData = null) : base(trainingId)
        {
            SimpleTrainingData = simpleTrainingData;
            HasTrainingId = !string.IsNullOrEmpty(trainingId);
            HaveSimpleTrainingData = SimpleTrainingData != null;
        }
    }
}