using Models;

namespace Data
{
    public class ExerciseDataService : BaseDataService<Exercise>
    {
        public ExerciseDataService(ISaveService saveService) : base(saveService.ExercisesCache)
        {
        }
    }
}