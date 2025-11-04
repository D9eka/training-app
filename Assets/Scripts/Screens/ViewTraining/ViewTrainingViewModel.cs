using System;
using System.Collections.Generic;
using System.Globalization;
using Data;
using Models;
using Screens.CreateTraining;
using Screens.ViewModels;

namespace Screens.ViewTraining
{
    public class ViewTrainingViewModel : IViewModel
    {
        private readonly TrainingDataService _trainingDataService;
        private readonly IDataService<Exercise> _exerciseDataService;

        private string _trainingId;
        private Training _currentTraining;

        public string TrainingId
        {
            get => _trainingId;
            set
            {
                if (_trainingId == value) return;
                _trainingId = value;
                Load();
            }
        }
        public string TrainingName { get; private set; }
        public string TrainingDescription { get; private set; }
        public string PrepTimeText { get; private set; }
        public IReadOnlyList<TrainingBlockViewData> BlocksViewData { get; private set; }
        public bool IsNotFound { get; private set; }

        public event Action TrainingChanged;

        public ViewTrainingViewModel(
            TrainingDataService trainingDataService,
            IDataService<Exercise> exerciseDataService,
            string trainingId)
        {
            _trainingDataService = trainingDataService ?? throw new ArgumentNullException(nameof(trainingDataService));
            _exerciseDataService = exerciseDataService ?? throw new ArgumentNullException(nameof(exerciseDataService));
            TrainingId = trainingId ?? throw new ArgumentException("Training ID cannot be null or empty", nameof(trainingId));

            _trainingDataService.DataUpdated += _ => Load();
            Load();
        }

        private void Load()
        {
            _currentTraining = _trainingDataService.GetDataById(TrainingId);
            UpdateUiData();
            TrainingChanged?.Invoke();
        }

        private void UpdateUiData()
        {
            if (_currentTraining == null)
            {
                IsNotFound = true;
                TrainingName = "Тренировка не найдена";
                TrainingDescription = "";
                PrepTimeText = "";
                return;
            }

            IsNotFound = false;
            TrainingName = _currentTraining.Name;
            TrainingDescription = $"Описание: {_currentTraining.Description}";
            PrepTimeText = _currentTraining.PrepTimeSeconds.ToString(CultureInfo.CurrentCulture);

            BlocksViewData = GetTrainingBlocks();
        }
        
        public IReadOnlyList<TrainingBlockViewData> GetTrainingBlocks()
        {
            List<TrainingBlockViewData> trainingBlocks = new List<TrainingBlockViewData>();

            foreach (var block in _currentTraining.Blocks)
            {
                List<ExerciseInBlockViewData> exerciseInBlockViewData = new List<ExerciseInBlockViewData>();
                foreach (var exerciseInBlock in block.Exercises)
                {
                    Exercise exercise = _exerciseDataService.GetDataById(exerciseInBlock.ExerciseId);
                    exerciseInBlockViewData.Add(new ExerciseInBlockViewData
                    {
                        Name = exercise.Name,
                        Repetitions = exerciseInBlock.Repetitions
                    });
                }
                trainingBlocks.Add(new TrainingBlockViewData
                {
                    Id = block.Id,
                    ExercisesInBlockViewData = exerciseInBlockViewData,
                    Approaches = block.Approaches,
                    ApproachesTimeSpan = block.ApproachesTimeSpan,
                    RestAfterApproachTimeSpan = block.RestAfterApproachTimeSpan,
                    Sets = block.Sets,
                    RestAfterSetsTimeSpan = block.RestAfterSetTimeSpan,
                    RestAfterBlockTimeSpan = block.RestAfterBlockTimeSpan
                
                });
            }
            
            return trainingBlocks;
        }

        public void DeleteTraining()
        {
            _trainingDataService.RemoveData(TrainingId);
        }
    }
}
