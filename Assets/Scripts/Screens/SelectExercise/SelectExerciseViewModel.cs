using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.ViewExercises;
using Screens.ViewModels;

namespace Screens.SelectExercise
{
    public class SelectExerciseViewModel : IUpdatableViewModel<SelectExerciseParameter>
    {
        private readonly TrainingDataService _trainingDataService;
        private readonly IDataService<Exercise> _exerciseDataService;
        private string _searchQuery = string.Empty;
    
        private Training _currentTraining;
        private TrainingBlock _currentBlock;
        private IReadOnlyList<Exercise> _allExercises;
        private int _exerciseIndex;

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
        public List<ExerciseViewData> ExercisesWithQuery { get; private set; }

        public SelectExerciseViewModel(TrainingDataService trainingDataService,
            IDataService<Exercise> exerciseDataService, SelectExerciseParameter param)
        {
            _trainingDataService = trainingDataService;
            _exerciseDataService =  exerciseDataService;

            _allExercises = _exerciseDataService.Cache;
            UpdateExercisesWithQueryList(SearchQuery);
            
            UpdateParameter(param);
            _exerciseDataService.DataUpdated += ExerciseDataServiceOnDataUpdated;
        }

        public void UpdateParameter(SelectExerciseParameter param)
        {
            _currentBlock = _trainingDataService.GetBlockById(param.BlockId);
            _currentTraining = _trainingDataService.GetDataById(_currentBlock.TrainingId);
            _exerciseIndex = param.ExerciseIndex;
        }

        public void Save(string exerciseId)
        {
            _currentBlock.AddExercise(new ExerciseInBlock(_exerciseDataService.GetDataById(exerciseId)), _exerciseIndex);
            _currentTraining.AddOrUpdateBlock(_currentBlock);
            _trainingDataService.UpdateData(_currentTraining);
            Clear();
        }

        private void ExerciseDataServiceOnDataUpdated(IReadOnlyList<Exercise> cache)
        {
            _allExercises = cache;
            UpdateExercisesWithQueryList(SearchQuery);
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

        private void Clear()
        {
            _currentTraining = null;
            _currentBlock = null;
            _exerciseIndex = -1;
            _allExercises = new List<Exercise>();
            SearchQuery = string.Empty;
            ExercisesWithQueryUpdated?.Invoke();
        }
    }
}
