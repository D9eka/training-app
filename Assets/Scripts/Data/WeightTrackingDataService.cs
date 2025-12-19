using Models;

namespace Data
{
    public class WeightTrackingDataService : BaseDataService<WeightTracking>
    {
        public WeightTrackingDataService(ISaveService saveService) : base(saveService.WeightsCache)
        {
        }
    }
}