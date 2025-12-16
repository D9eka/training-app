using Data;
using Models;
using Screens.AddWeight;
using Screens.Factories.Parameters;

namespace Screens.Factories
{
    public class AddWeightFactory : IViewModelFactory<AddWeightViewModel, IScreenParameter>
    {
        private readonly IDataService<WeightTracking> _weightTrackingDataService;

        public AddWeightFactory(IDataService<WeightTracking> weightTrackingDataService)
        {
            _weightTrackingDataService = weightTrackingDataService;
        }

        public AddWeightViewModel Create(IScreenParameter param)
        {
            return new AddWeightViewModel(_weightTrackingDataService);
        }
    }
}