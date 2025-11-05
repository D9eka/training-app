namespace Screens.Factories.Parameters
{
    public class ExerciseIdParameter : IScreenParameter
    {
        public string ExerciseId { get; private set; }
        
        public ExerciseIdParameter(string exerciseId)
        {
            ExerciseId = exerciseId;
        }
    }
}