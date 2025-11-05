using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.ViewExercise;

namespace Screens.Factories
{
    public sealed class ViewExerciseFactory 
        : IViewModelFactory<ViewExerciseViewModel, ExerciseIdParameter>
    {
        private readonly IDataService<Exercise> _exerciseService;

        public ViewExerciseFactory(IDataService<Exercise> exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public ViewExerciseViewModel Create(ExerciseIdParameter param)
        {
            return new ViewExerciseViewModel(_exerciseService, param);
        }
    }
}