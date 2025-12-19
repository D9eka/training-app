using System;
using Screens.ViewModels;

namespace Screens.CreateSimpleTraining
{
    public class CreateSimpleTrainingViewModel : IViewModel
    {
        public event Action DataUpdated;

        public int PreparingSeconds { get; set; } = 15;
        public int ApproachesDurationSeconds { get; set; }
        public int RestAfterApproachSeconds { get; set; }
        public int Approaches { get; set; } = 1;
        public int Sets { get; set; } = 1;
        public int RestAfterSetSeconds { get; set; }

        public void Clear()
        {
            PreparingSeconds = 15;
            ApproachesDurationSeconds = 0;
            RestAfterApproachSeconds = 0;
            Approaches = 1;
            Sets = 1;
            RestAfterSetSeconds = 0;
            DataUpdated?.Invoke();
        }

        public SimpleTrainingData GetSimpleTraining()
        {
            return new SimpleTrainingData(
                PreparingSeconds,
                ApproachesDurationSeconds,
                RestAfterApproachSeconds,
                Approaches,
                Sets,
                RestAfterSetSeconds
            );
        }
    }
}