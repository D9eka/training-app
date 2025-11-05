namespace Screens.Factories.Parameters
{
    public class SelectExerciseParameter : BlockIdParameter
    {
        public int ExerciseIndex { get; private set; }
        
        public SelectExerciseParameter(string blockId, int exerciseIndex = -1) : base(blockId)
        {
            ExerciseIndex = exerciseIndex;
        }
    }
}