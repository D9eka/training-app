using Data;
using Screens.Factories.Parameters;
using Screens.Main;

namespace Screens.Factories
{
    public class MainFactory : IViewModelFactory<MainViewModel, IScreenParameter>
    {
        private readonly TrainingDataService _trainingService;

        public MainFactory(TrainingDataService trainingService)
        {
            _trainingService = trainingService;
        }
        
        public MainViewModel Create(IScreenParameter param)
        {
            return new MainViewModel(_trainingService, param);
        }
    }
}