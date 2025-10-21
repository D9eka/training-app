using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.CreateExercise;
using Screens.ViewModels;

namespace Screens.CreateTraining
{
    public class CreateTrainingViewModel : IViewModel
    {
        public bool IsEditMode { get; private set; }
        
        private readonly IDataService<Training> _trainingDataService;
        private string _name = string.Empty;
    
        private Training _currentTraining;

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
        public float PrepTimeSeconds { get; set; }
        private List<TrainingBlock> _trainingBlocks = new List<TrainingBlock>();
        public IReadOnlyList<TrainingBlockViewData> TrainingBlocks => _trainingBlocks.Select(block =>
            new TrainingBlockViewData
            {
                Id = block.Id,
                ExercisesInBlockViewData = block.Exercises.Select(ex => 
                    new ExerciseInBlockViewData
                    {
                        Name = ex.Exercise.Name,
                        Repetitions = ex.Repetitions
                    }).ToList(),
                Approaches = block.Approaches,
                ApproachesTimeSpan = block.ApproachesTimeSpan,
                RestAfterApproachTimeSpan = block.RestAfterApproachTimeSpan,
                Sets = block.Sets,
                RestAfterSetsTimeSpan = block.RestAfterSetTimeSpan,
                RestAfterBlockTimeSpan = block.RestAfterBlockTimeSpan
                
            }).ToList();
        
        public bool CanSave => !string.IsNullOrWhiteSpace(Name);

        public CreateTrainingViewModel(IDataService<Training> trainingDataService, string trainingId)
        {
            _trainingDataService =  trainingDataService;
            UpdateId(trainingId);
            _trainingDataService.DataUpdated += TrainingDataServiceOnDataUpdated;
        }

        public void UpdateId(string trainingId)
        {
            _currentTraining = GetTrainingById(trainingId);
            TrainingId = _currentTraining.Id;
            Load();
        }

        private Training GetTrainingById(string trainingId)
        {
            Training training = _trainingDataService.GetDataById(trainingId);
            IsEditMode = training != null;
            EditModeChanged?.Invoke(IsEditMode);
            return training ?? new Training();
        }

        private void Load()
        {
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
    }
}
