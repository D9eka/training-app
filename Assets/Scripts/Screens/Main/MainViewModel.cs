using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IDataService<WeightTracking> _weightTrackingDataService;

        public IReadOnlyList<TrainingViewData> LastTrainings { get; private set; } = new List<TrainingViewData>();
        public IReadOnlyList<float> WeekWeights { get; private set; } = new List<float>();

        public Action DataUpdated;
        
        public MainViewModel(TrainingDataService trainingService, 
            IDataService<WeightTracking> weightTrackingDataService)
        {
            _trainingDataService =  trainingService;
            _weightTrackingDataService = weightTrackingDataService;
            _trainingDataService.DataUpdated += _ => UpdateParameter(default);
            _weightTrackingDataService.DataUpdated += _ => UpdateParameter(default);
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

            LastTrainings = GetLastTrainings();
            WeekWeights = GetLastWeekWeights();
            
            DataUpdated?.Invoke();
        }

        private List<TrainingViewData> GetLastTrainings()
        {
            int lastTrainingsCount = Math.Min(_trainingDataService.Cache.Count, LAST_TRAININGS_COUNT);
            if (lastTrainingsCount == 0) return new List<TrainingViewData>();
            return _trainingDataService.Cache
                .OrderByDescending(training => training.LastTime)
                .Take(lastTrainingsCount)
                .Select(training => new TrainingViewData(training))
                .ToList();
        }

        private List<float> GetLastWeekWeights()
        {
            IReadOnlyList<WeightTracking> weightData = _weightTrackingDataService.Cache;

            if (weightData == null || weightData.Count == 0)
                return new List<float>();

            DateTime fromDate = DateTime.Now.Date.AddDays(-7 + 1);

            return weightData
                .Where(w => w.Time >= fromDate)
                .OrderBy(w => w.Time)
                .Select(w => w.Weight)
                .ToList();
        }
    }
}
