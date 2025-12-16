using System;
using System.Linq;
using Models;

namespace Screens.ViewTrainings
{
    public class TrainingViewData
    {
        public string Id;
        public string Name;
        public int ExerciseCount;
        public TimeSpan Duration;
        public DateTime LastTime;
        
        public TrainingViewData(string id, string name, int exerciseCount, TimeSpan duration, DateTime lastTime)
        {
            Id = id;
            Name = name;
            ExerciseCount = exerciseCount;
            Duration = duration;
            LastTime = lastTime;
        }

        public TrainingViewData(Training training) : this(
            training.Id,
            training.Name,
            training.Blocks.Sum(b => b.Exercises.Count),
            training.Duration,
            training.LastTime)
        {
        }
    }
}