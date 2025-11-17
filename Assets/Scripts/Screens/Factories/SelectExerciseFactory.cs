using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.SelectExercise;

namespace Screens.Factories
{
    public class SelectExerciseFactory : IViewModelFactory<SelectExerciseViewModel, SelectExerciseParameter>
    {
        private readonly TrainingDataService _trainingService;
        private readonly IDataService<Exercise> _exerciseService;
        private readonly IDataService<Equipment> _equipmentService;

        public SelectExerciseFactory(
            TrainingDataService trainingService,
            IDataService<Exercise> exerciseService,
            IDataService<Equipment> equipmentService)
        {
            _trainingService = trainingService;
            _exerciseService = exerciseService;
            _equipmentService = equipmentService;
        }

        public SelectExerciseViewModel Create(SelectExerciseParameter param)
        {
            return new SelectExerciseViewModel(_trainingService, _exerciseService, _equipmentService, param);
        }
    }
}
