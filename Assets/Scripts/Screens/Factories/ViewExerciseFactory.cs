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
        private readonly IDataService<Equipment> _equipmentService;

        public ViewExerciseFactory(
            IDataService<Exercise> exerciseService,
            IDataService<Equipment> equipmentService)
        {
            _exerciseService = exerciseService;
            _equipmentService = equipmentService;
        }

        public ViewExerciseViewModel Create(ExerciseIdParameter param)
        {
            return new ViewExerciseViewModel(_exerciseService, _equipmentService, param);
        }
    }
}
