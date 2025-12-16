using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.Main;

namespace Screens.Factories
{
    public class MainFactory : IViewModelFactory<MainViewModel, IScreenParameter>
    {
        private readonly TrainingDataService _trainingService;
        private readonly IDataService<WeightTracking> _weightTrackingDataService;

        public MainFactory(TrainingDataService trainingService, IDataService<WeightTracking> weightTrackingDataService)
        {
            _trainingService = trainingService;
            _weightTrackingDataService = weightTrackingDataService;
        }

        public MainViewModel Create(IScreenParameter param)
        {
            return new MainViewModel(_trainingService, _weightTrackingDataService, param);
        }
    }
}