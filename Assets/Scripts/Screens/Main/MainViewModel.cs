using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.ViewTrainings;

namespace Screens.Main
{
    public class MainViewModel : IUpdatableViewModel<IScreenParameter>
    {
        private const int LAST_TRAININGS_COUNT = 5;
        
        private readonly TrainingDataService _trainingDataService;

        public List<TrainingViewData> LastTrainings { get; private set; } = new List<TrainingViewData>();

        public Action DataUpdated;
        
        public MainViewModel(TrainingDataService trainingService, IScreenParameter param)
        {
            _trainingDataService =  trainingService;
            UpdateParameter(param);
            _trainingDataService.DataUpdated += _ => UpdateParameter(default);
        }
        
        public void UpdateParameter(IScreenParameter param)
        {
            int lastTrainingsCount = Math.Min(_trainingDataService.Cache.Count, LAST_TRAININGS_COUNT);
            if (lastTrainingsCount == 0) return;
            LastTrainings = _trainingDataService.Cache
                .OrderByDescending(training => training.LastTime)
                .Take(lastTrainingsCount)
                .Select(training => new TrainingViewData(training))
                .ToList();
            
            DataUpdated?.Invoke();
        }
    }
}
