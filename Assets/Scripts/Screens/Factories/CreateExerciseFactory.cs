using Data;
using Models;
using Screens.CreateExercise;
using Screens.Factories.Parameters;

namespace Screens.Factories
{
    public class CreateExerciseFactory : IViewModelFactory<CreateExerciseViewModel, ExerciseIdParameter>
    {
        private readonly IDataService<Exercise> _exerciseService;
        private readonly IDataService<Equipment> _equipmentService;

        public CreateExerciseFactory(IDataService<Exercise> exerciseService, IDataService<Equipment> equipmentService)
        {
            _exerciseService = exerciseService;
            _equipmentService = equipmentService;
        }
        
        public CreateExerciseViewModel Create(ExerciseIdParameter param)
        {
            return new CreateExerciseViewModel(_exerciseService, _equipmentService, param);
        }
    }
}