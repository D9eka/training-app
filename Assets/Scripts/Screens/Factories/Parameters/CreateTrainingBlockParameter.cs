namespace Screens.Factories.Parameters
{
    public class CreateTrainingBlockParameter : TrainingIdParameter
    {
        public string? BlockId { get; }

        public bool HasBlockId => !string.IsNullOrEmpty(BlockId);

        public CreateTrainingBlockParameter(string trainingId, string blockId = null) : base(trainingId)
        {
            BlockId = blockId;
        }
    }
}