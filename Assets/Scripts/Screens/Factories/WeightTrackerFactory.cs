using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.WeightTracker;

namespace Screens.Factories
{
    public class WeightTrackerFactory : IViewModelFactory<WeightTrackerViewModel, IScreenParameter>
    {
        private readonly IDataService<WeightTracking> _weightTrackingDataService;

        public WeightTrackerFactory(IDataService<WeightTracking> weightTrackingDataService)
        {
            _weightTrackingDataService = weightTrackingDataService;
        }

        public WeightTrackerViewModel Create(IScreenParameter param)
        {
            return new WeightTrackerViewModel(_weightTrackingDataService);
        }
    }
}