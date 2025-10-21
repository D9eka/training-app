using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.ViewExercises;
using Screens.ViewModels;

namespace Screens.SelectExercise
{
    public class SelectExerciseViewModel : IViewModel
    {
        private readonly IDataService<Exercise> _exerciseDataService;
        private readonly TrainingDataService _trainingDataService;
        private string _searchQuery = string.Empty;
    
        private Training _currentTraining;
        private TrainingBlock _currentBlock;

        public event Action ExercisesWithQueryUpdated;

        public string SearchQuery
        {
            get => _searchQuery;
            set 
            { 
                if (value == _searchQuery) return;
                _searchQuery = value;
                UpdateExercisesWithQueryList(_searchQuery);
            }
        }

        private IReadOnlyList<Exercise> _allExercises;
        public List<ExerciseViewData> ExercisesWithQuery { get; private set; }

        public SelectExerciseViewModel(IDataService<Exercise> exerciseDataService, 
            TrainingDataService trainingDataService, string blockId)
        {
            _exerciseDataService =  exerciseDataService;
            _trainingDataService = trainingDataService;

            _allExercises = _exerciseDataService.Cache;
            UpdateExercisesWithQueryList(SearchQuery);
            
            UpdateId(blockId);
            _exerciseDataService.DataUpdated += ExerciseDataServiceOnDataUpdated;
        }

        public void UpdateId(string blockId)
        {
            _currentBlock = _trainingDataService.GetBlockById(blockId);
            _currentTraining = _trainingDataService.GetDataById(_currentBlock.TrainingId);
        }

        private void ExerciseDataServiceOnDataUpdated(IReadOnlyList<Exercise> cache)
        {
            _allExercises = cache;
            UpdateExercisesWithQueryList(SearchQuery);
            ExercisesWithQueryUpdated?.Invoke();
        }

        private void UpdateExercisesWithQueryList(string searchQuery)
        {
            ExercisesWithQuery = new List<ExerciseViewData>();
            foreach (var exercise in _allExercises)
            {
                if (exercise.Name.ToLower().Contains(searchQuery.ToLower()))
                {
                    ExercisesWithQuery.Add(new ExerciseViewData
                    {
                        Id = exercise.Id,
                        Name = exercise.Name,
                        Equipments = exercise.RequiredEquipment.Select(req =>
                            (req.Equipment?.Name ?? "???", req.Quantity)
                        ).ToList()
                    });
                }
            }
            ExercisesWithQueryUpdated?.Invoke();
        }

        public void Save(string exerciseId)
        {
            _currentBlock.AddExercise(new ExerciseInBlock(_exerciseDataService.GetDataById(exerciseId)));
            _currentTraining.AddOrUpdateBlock(_currentBlock);
        }
    }
}
