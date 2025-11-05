using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.ViewTraining;

namespace Screens.Factories
{
    public class ViewTrainingFactory : IViewModelFactory<ViewTrainingViewModel, TrainingIdParameter>
    {
        private readonly TrainingDataService _trainingService;
        private readonly IDataService<Exercise> _exerciseService;

        public ViewTrainingFactory(TrainingDataService trainingService, IDataService<Exercise> exerciseService)
        {
            _trainingService = trainingService;
            _exerciseService = exerciseService;
        }
        
        public ViewTrainingViewModel Create(TrainingIdParameter param)
        {
            return new ViewTrainingViewModel(_trainingService, _exerciseService, param);
        }
    }
}