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

        public SelectExerciseFactory(TrainingDataService trainingService, IDataService<Exercise> exerciseService)
        {
            _trainingService = trainingService;
            _exerciseService = exerciseService;
        }

        public SelectExerciseViewModel Create(SelectExerciseParameter param)
        {
            return new SelectExerciseViewModel(_trainingService, _exerciseService, param);
        }
    }
}