using System.Collections.Generic;
using Models;

namespace Data
{
    public class TrainingDataService : BaseDataService<Training>
    {
        public TrainingDataService(List<Training> cache) : base(cache)
        {
        }
    }
}