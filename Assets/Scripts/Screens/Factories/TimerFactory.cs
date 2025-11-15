using Core;
using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.Timer;

namespace Screens.Factories
{
    public class TimerFactory : IViewModelFactory<TimerViewModel, TrainingIdParameter>
    {
        private readonly TrainingDataService _trainingService;
        private readonly IDataService<Exercise> _exerciseService;
        private readonly IDataService<Equipment> _equipmentService;
        private readonly TickableManager _tickableManager;

        public TimerFactory(TrainingDataService trainingService, IDataService<Exercise> exerciseService,
            IDataService<Equipment> equipmentService, TickableManager tickableManager)
        {
            _trainingService = trainingService;
            _exerciseService = exerciseService;
            _equipmentService = equipmentService;
            _tickableManager = tickableManager;
        }

        public TimerViewModel Create(TrainingIdParameter param)
        {
            TimerViewModel vm = new TimerViewModel(_trainingService, _exerciseService, _equipmentService, param);
            _tickableManager.Register(vm);
            return vm;
        }
    }
}