using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.ViewExercises;

namespace Screens.Factories
{
    public class ViewExercisesFactory : IViewModelFactory<ViewExercisesViewModel, IScreenParameter>
    {
        private readonly IDataService<Exercise> _exerciseService;
        private readonly IDataService<Equipment> _equipmentService;

        public ViewExercisesFactory(IDataService<Exercise> exerciseService, IDataService<Equipment> equipmentService)
        {
            _exerciseService = exerciseService;
            _equipmentService = equipmentService;
        }
        
        public ViewExercisesViewModel Create(IScreenParameter param)
        {
            return new ViewExercisesViewModel(_exerciseService, _equipmentService);
        }
    }
}