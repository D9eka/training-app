using System;
using System.Collections.Generic;
using Data;
using Models;

namespace Screens.ViewExercise
{
    public class ViewExerciseViewModel
    {
        private readonly IDataService<Exercise> _exerciseDataService;
        private readonly string _exerciseId;
        private Exercise _currentExercise;

        public Exercise CurrentExercise => _currentExercise;
        public event Action ExerciseChanged;

        public ViewExerciseViewModel(IDataService<Exercise> exerciseDataService, string exerciseId)
        {
            _exerciseDataService = exerciseDataService ?? throw new ArgumentNullException(nameof(exerciseDataService));
            _exerciseId = exerciseId ?? throw new ArgumentException("Exercise ID cannot be null or empty", nameof(exerciseId));
            Load();
            _exerciseDataService.DataUpdated += Load;
        }

        private void Load(IReadOnlyList<Exercise> allExercises)
        {
            foreach (var exercise in allExercises)
            {
                if (exercise.Id == _exerciseId)
                {
                    _currentExercise = exercise;
                }
            }
        }

        private void Load()
        {
            _currentExercise = _exerciseDataService.GetDataById(_exerciseId);
            ExerciseChanged?.Invoke();
        }

        public void DeleteExercise()
        {
            _exerciseDataService.RemoveData(_exerciseId);
        }
    }
}