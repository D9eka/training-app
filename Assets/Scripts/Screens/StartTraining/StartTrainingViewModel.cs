using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.ViewModels;
using Screens.ViewTrainings;

namespace Screens.StartTraining
{
    public class StartTrainingViewModel : IViewModel
    {
        private readonly TrainingDataService _trainingDataService;
        
        private string _searchQuery = string.Empty;
        private IReadOnlyList<Training> _allTrainings;

        public event Action TrainingsWithQueryUpdated;

        public string SearchQuery
        {
            get => _searchQuery;
            set 
            { 
                if (value == _searchQuery) return;
                _searchQuery = value;
                UpdateTrainingsWithQueryList(_searchQuery);
            }
        }
        public List<TrainingViewData> TrainingsWithQuery { get; private set; }

        public StartTrainingViewModel(TrainingDataService trainingDataService)
        {
            _trainingDataService = trainingDataService;

            _allTrainings = _trainingDataService.Cache;
            UpdateTrainingsWithQueryList(SearchQuery);
            
            _trainingDataService.DataUpdated += TrainingDataServiceOnDataUpdated;
        }

        public void Clear()
        {
            _allTrainings = new List<Training>();
            SearchQuery = string.Empty;
            TrainingsWithQueryUpdated?.Invoke();
        }

        private void TrainingDataServiceOnDataUpdated(IReadOnlyList<Training> cache)
        {
            _allTrainings = cache;
            UpdateTrainingsWithQueryList(SearchQuery);
        }

        private void UpdateTrainingsWithQueryList(string searchQuery)
        {
            TrainingsWithQuery = new List<TrainingViewData>();
            foreach (Training training in _allTrainings)
            {
                if (training.Name.ToLower().Contains(searchQuery.ToLower()))
                {
                    TrainingsWithQuery.Add(new TrainingViewData(training));
                }
            }
            TrainingsWithQueryUpdated?.Invoke();
        }
    }
}