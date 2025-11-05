using Data;
using Models;
using Screens.CreateTraining;
using Screens.Factories.Parameters;

namespace Screens.Factories
{
    public class CreateTrainingFactory : IViewModelFactory<CreateTrainingViewModel, TrainingIdParameter>
    {
        private readonly TrainingDataService _trainingService;
        private readonly IDataService<Exercise> _exerciseService;

        public CreateTrainingFactory(TrainingDataService trainingService, IDataService<Exercise> exerciseService)
        {
            _trainingService = trainingService;
            _exerciseService = exerciseService;
        }

        public CreateTrainingViewModel Create(TrainingIdParameter param)
        {
            return new CreateTrainingViewModel(_trainingService, _exerciseService, param);
        }
    }
}