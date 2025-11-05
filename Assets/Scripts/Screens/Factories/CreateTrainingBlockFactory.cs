using Data;
using Models;
using Screens.CreateBlock;
using Screens.Factories.Parameters;

namespace Screens.Factories
{
    public class CreateTrainingBlockFactory : 
        IViewModelFactory<CreateTrainingBlockViewModel, CreateTrainingBlockParameter>
    {
        private readonly TrainingDataService _trainingService;
        private readonly IDataService<Exercise> _exerciseService;

        public CreateTrainingBlockFactory(TrainingDataService trainingService, IDataService<Exercise> exerciseService)
        {
            _trainingService = trainingService;
            _exerciseService = exerciseService;
        }

        public CreateTrainingBlockViewModel Create(CreateTrainingBlockParameter param)
        {
            return new CreateTrainingBlockViewModel(_trainingService, _exerciseService, param);
        }
    }
}