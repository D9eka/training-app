using Models;

namespace Data
{
    public class TrainingDataService : BaseDataService<Training>
    {
        public TrainingDataService(ISaveService saveService) : base(saveService.TrainingsCache)
        {
        }
        
        public TrainingBlock GetBlockById(string trainingBlockId)
        {
            foreach (Training training in Cache)
            {
                foreach (TrainingBlock block in training.Blocks)
                {
                    if (block.Id == trainingBlockId)
                    {
                        return block;
                    }
                }
            }
            return null;
        }
    }
}