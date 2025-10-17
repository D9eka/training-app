using System;
using System.Collections.Generic;
using Data;
using Models;

namespace Screens.ViewExercises
{
    public class ViewExercisesViewModel
    {
        private readonly IDataService<Exercise> _exerciseDataService;
        private IReadOnlyList<Exercise> _exercises;

        public IReadOnlyList<Exercise> Exercises => _exercises;
        public event Action ExercisesChanged;

        public ViewExercisesViewModel(IDataService<Exercise> exerciseDataService)
        {
            _exerciseDataService = exerciseDataService ?? throw new ArgumentNullException(nameof(exerciseDataService));
            Load(_exerciseDataService.Cache);
            _exerciseDataService.DataUpdated += Load;
        }

        private void Load(IReadOnlyList<Exercise> allExercises)
        {
            _exercises = allExercises;
            ExercisesChanged?.Invoke();
        }
    }
}