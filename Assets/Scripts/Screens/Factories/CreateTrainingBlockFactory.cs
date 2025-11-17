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
        private readonly IDataService<Equipment> _equipmentService;

        public CreateTrainingBlockFactory(
            TrainingDataService trainingService,
            IDataService<Exercise> exerciseService,
            IDataService<Equipment> equipmentService)
        {
            _trainingService = trainingService;
            _exerciseService = exerciseService;
            _equipmentService = equipmentService;
        }

        public CreateTrainingBlockViewModel Create(CreateTrainingBlockParameter param)
        {
            return new CreateTrainingBlockViewModel(_trainingService, _exerciseService, _equipmentService, param);
        }
    }
}
