using Data;
using Screens.Factories.Parameters;
using Screens.StartTraining;

namespace Screens.Factories
{
    public class StartTrainingFactory : IViewModelFactory<StartTrainingViewModel, IScreenParameter>
    {
        private readonly TrainingDataService _trainingService;

        public StartTrainingFactory(TrainingDataService trainingService)
        {
            _trainingService = trainingService;
        }
        
        public StartTrainingViewModel Create(IScreenParameter param)
        {
            return new StartTrainingViewModel(_trainingService);
        }
    }
}