using System;
using System.Collections.Generic;
using Data;
using Models;

namespace Screens.ViewExercises
{
    /// <summary>
    /// ViewModel для списка упражнений.
    /// </summary>
    public class ViewExercisesViewModel
    {
        private readonly IDataService _dataService;
        private IReadOnlyList<Exercise> _exercises;

        public IReadOnlyList<Exercise> Exercises => _exercises;
        public event Action ExercisesChanged;

        public ViewExercisesViewModel(IDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            Load();
            _dataService.ExercisesUpdated += Load;
        }

        public void Load()
        {
            _exercises = _dataService.GetAllExercises() ?? new List<Exercise>().AsReadOnly();
            ExercisesChanged?.Invoke();
        }
    }
}