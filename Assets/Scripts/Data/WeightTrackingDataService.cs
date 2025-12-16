using System.Collections.Generic;
using Models;

namespace Data
{
    public class WeightTrackingDataService : BaseDataService<WeightTracking>
    {
        public WeightTrackingDataService(List<WeightTracking> cache) : base(cache)
        {
        }
    }
}