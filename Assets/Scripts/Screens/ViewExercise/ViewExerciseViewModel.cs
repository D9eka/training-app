using System;
using Models;

namespace Screens.ViewExercise
{
    public class ViewExerciseViewModel
    {
        public event Action ExerciseChanged;

        public Exercise CurrentExercise { get; private set; }

        public void SetExercise(Exercise exercise)
        {
            CurrentExercise = exercise;
            ExerciseChanged?.Invoke();
        }
    }
}