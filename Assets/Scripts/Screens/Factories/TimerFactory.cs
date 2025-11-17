using Core;
using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.Timer;

namespace Screens.Factories
{
    public class TimerFactory : IViewModelFactory<TimerViewModel, TimerParameter>
    {
        private readonly TrainingDataService _trainingService;
        private readonly IDataService<Exercise> _exerciseService;
        private readonly IDataService<Equipment> _equipmentService;
        private readonly UiController _uiController;
        private readonly TickableManager _tickableManager;

        public TimerFactory(TrainingDataService trainingService, 
            IDataService<Exercise> exerciseService, IDataService<Equipment> equipmentService, 
            UiController uiController, TickableManager tickableManager)
        {
            _trainingService = trainingService;
            _exerciseService = exerciseService;
            _equipmentService = equipmentService;
            _uiController = uiController;
            _tickableManager = tickableManager;
        }

        public TimerViewModel Create(TimerParameter param)
        {
            TimerViewModel vm = new TimerViewModel(_trainingService, _exerciseService, 
                _equipmentService, _uiController, param);
            _tickableManager.Register(vm);
            return vm;
        }
    }
}