using System.Collections.Generic;
using Models;

namespace Data
{
    public class ExerciseDataService : BaseDataService<Exercise>
    {
        public ExerciseDataService(List<Exercise> cache) : base(cache)
        {
        }
    }
}