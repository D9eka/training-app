using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.ViewTrainings;

namespace Screens.ViewStats
{
    public class ViewStatsViewModel : IUpdatableViewModel<IScreenParameter>
    {
        private readonly TrainingDataService _trainingDataService;
        private readonly IDataService<WeightTracking> _weightTrackingDataService;
        
        public IReadOnlyList<TrainingViewData> PreviousTrainings { get; private set; } = new List<TrainingViewData>();
        public IReadOnlyList<float> WeekWeights { get; private set; } = new List<float>();

        public Action DataUpdated;
        
        public ViewStatsViewModel(TrainingDataService trainingService, 
            IDataService<WeightTracking> weightTrackingDataService)
        {
            _trainingDataService =  trainingService;
            _weightTrackingDataService = weightTrackingDataService;
            _trainingDataService.DataUpdated += _ => UpdateParameter(default);
            _weightTrackingDataService.DataUpdated += _ => UpdateParameter(default);
        }
        
        public void UpdateParameter(IScreenParameter param)
        {
            PreviousTrainings = GetPreviousTrainings();
            WeekWeights = GetLastWeekWeights();
            
            DataUpdated?.Invoke();
        }

        private List<TrainingViewData> GetPreviousTrainings()
        {
            if (_trainingDataService.Cache.Count == 0) return new List<TrainingViewData>();
            return _trainingDataService.Cache
                .OrderByDescending(training => training.LastTime)
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