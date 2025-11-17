using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.CreateExercise;
using Screens.Factories.Parameters;
using Screens.ViewModels;

namespace Screens.CreateTraining
{
    public class CreateTrainingViewModel : IUpdatableViewModel<TrainingIdParameter>
    {
        public bool IsEditMode { get; private set; }
        
        private readonly IDataService<Training> _trainingDataService;
        private readonly IDataService<Exercise> _exerciseDataService;
        
        private Training _currentTraining;
        private List<TrainingBlock> _trainingBlocks = new List<TrainingBlock>();
        private string _name = string.Empty;

        public event Action<bool> EditModeChanged;
        public event Action<bool> CanSaveChanged;
        public event Action TrainingChanged;

        public string TrainingId { get; private set; }
        
        public string Name
        {
            get => _name;
            set { _name = value ?? string.Empty; CanSaveChanged?.Invoke(CanSave); }
        }

        public string Description { get; set; }
        public int PrepTimeSeconds { get; set; } = 15;

        public IReadOnlyList<TrainingBlockViewData> GetTrainingBlocks()
        {
            List<TrainingBlockViewData> trainingBlocks = new List<TrainingBlockViewData>();

            foreach (var block in _trainingBlocks)
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
                    RestAfterApproachTimeSpan = block.RestAfterApproachTimeSpan,
                    Sets = block.Sets,
                    RestAfterSetsTimeSpan = block.RestAfterSetTimeSpan,
                    RestAfterBlockTimeSpan = block.RestAfterBlockTimeSpan
                
                });
            }
            
            return trainingBlocks;
        }
        
        public bool CanSave => !string.IsNullOrWhiteSpace(Name);

        public CreateTrainingViewModel(IDataService<Training> trainingDataService, 
            IDataService<Exercise> exerciseDataService, TrainingIdParameter param)
        {
            _trainingDataService =  trainingDataService;
            _exerciseDataService = exerciseDataService;
            UpdateParameter(param);
            _trainingDataService.DataUpdated += TrainingDataServiceOnDataUpdated;
        }

        public void UpdateParameter(TrainingIdParameter param)
        {
            IsEditMode = param != null;
            EditModeChanged?.Invoke(IsEditMode);
            _currentTraining = param != null ? _trainingDataService.GetDataById(param.TrainingId) : new Training();
            TrainingId = _currentTraining.Id;
            Load();
        }

        public void RemoveBlock(string blockId)
        {
            _trainingBlocks.Remove(_trainingBlocks.Find(block => block.Id == blockId));
            TrainingChanged?.Invoke();
        }

        public void Save()
        {
            if (!CanSave) return;
            _currentTraining.Name = Name;
            _currentTraining.Description = Description;
            _currentTraining.PrepTimeSeconds = PrepTimeSeconds;
            
            List<TrainingBlock> blocksCopy = _trainingBlocks.ToList();
            foreach (TrainingBlock trainingBlock in blocksCopy)
            {
                _currentTraining.AddOrUpdateBlock(trainingBlock);
            }

            if (_trainingDataService.GetDataById(TrainingId) != null)
            {
                _trainingDataService.UpdateData(_currentTraining);
            }
            else
            {
                _trainingDataService.AddData(_currentTraining);
            }
        }

        public void OnCreate()
        {
            Save();
            Clear();
        }

        private void Load()
        {
            if (_currentTraining == null) return;
            Name = _currentTraining.Name;
            Description = _currentTraining.Description;
            PrepTimeSeconds = _currentTraining.PrepTimeSeconds;
            _trainingBlocks = _currentTraining.Blocks;
            TrainingChanged?.Invoke();
        }

        private void TrainingDataServiceOnDataUpdated(IReadOnlyList<Training> trainings)
        {
            Load();
        }

        private void Clear()
        {
            _currentTraining = null;
            _trainingBlocks = new List<TrainingBlock>();
            TrainingId = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            PrepTimeSeconds = 15;
            TrainingChanged?.Invoke();
        }
    }
}
