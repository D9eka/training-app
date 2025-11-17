using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.ViewModels;

namespace Screens.ViewTrainings
{
    public class ViewTrainingsViewModel : IViewModel
    {
        private readonly IDataService<Training> _trainingDataService;

        public IReadOnlyList<TrainingViewData> Trainings { get; private set; }

        public event Action TrainingsChanged;

        public ViewTrainingsViewModel(IDataService<Training> trainingDataService)
        {
            _trainingDataService = trainingDataService ?? throw new ArgumentNullException(nameof(trainingDataService));

            Load(_trainingDataService.Cache);
            _trainingDataService.DataUpdated += Load;
        }

        private void Load(IReadOnlyList<Training> allTrainings)
        {
            Trainings = allTrainings
                .Select(training => new TrainingViewData(training))
                .ToList();

            TrainingsChanged?.Invoke();
        }
    }
}