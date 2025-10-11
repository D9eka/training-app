using System;
using Data;
using Models;

namespace Screens.ViewExercise
{
    /// <summary>
    /// ViewModel для просмотра одного упражнения.
    /// </summary>
    public class ViewExerciseViewModel
    {
        private readonly IDataService _dataService;
        private readonly string _exerciseId;
        private Exercise _currentExercise;

        public Exercise CurrentExercise => _currentExercise;
        public event Action ExerciseChanged;

        public ViewExerciseViewModel(IDataService dataService, string exerciseId)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _exerciseId = exerciseId ?? throw new ArgumentException("Exercise ID cannot be null or empty", nameof(exerciseId));
            Load();
            _dataService.ExercisesUpdated += Load;
        }

        private void Load()
        {
            _currentExercise = _dataService.GetExerciseById(_exerciseId);
            ExerciseChanged?.Invoke();
        }

        public void DeleteExercise()
        {
            _dataService.RemoveExercise(_exerciseId);
        }
    }
}