using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.CreateTraining;
using Screens.ViewModels;
using Unity.VisualScripting;
using UnityEngine;
using Views.Components;

namespace Screens.CreateBlock
{
    public class CreateTrainingBlockViewModel : IViewModel
    {
        public bool IsEditMode { get; private set; }
        
        private readonly TrainingDataService _trainingDataService;
    
        private Training _currentTraining;
        private TrainingBlock _currentBlock;

        public event Action<bool> EditModeChanged;
        public event Action<bool> CanSaveChanged;
        public event Action BlockChanged;
        
        private List<ExerciseInBlock> _exercises = new List<ExerciseInBlock>();
        
        public string BlockId { get; private set; }
        public IReadOnlyList<ExerciseInBlockViewData> Exercises => _exercises.Select(exercise =>
            new ExerciseInBlockViewData
            {
                Id = exercise.Id,
                Name = exercise.Exercise.Name,
                Equipments = exercise.Exercise.RequiredEquipment.Select(eq => new EquipmentInBlockViewData
                {
                    Id = eq.Equipment.Id,
                    Name = eq.Equipment.Name,
                    Quantity = eq.Quantity,
                    NeedWeight = eq.Equipment.HasWeight
                }).ToList(),
                Repetitions = exercise.Repetitions,
                DurationSeconds = exercise.DurationTimeSpan.Seconds
            }).ToList();
        public int Approaches { get; set; }
        public TimeSpan ApproachesTimeSpan { get; set; }
        public int Sets { get; set; }
        public TimeSpan RestAfterApproachTimeSpan { get; set; }
        public TimeSpan RestAfterSetTimeSpan { get; set; }
        public TimeSpan RestAfterBlockTimeSpan { get; set; }
        
        public bool CanSave => _exercises.Count > 0;

        public CreateTrainingBlockViewModel(TrainingDataService trainingDataService, string id)
        {
            _trainingDataService =  trainingDataService;
            UpdateId(id);
            _trainingDataService.DataUpdated += TrainingDataServiceOnDataUpdated;
        }

        public void UpdateId(string id)
        {
            _currentBlock = GetBlockById(id);
            BlockId = _currentBlock.Id;
            Load();
        }

        private TrainingBlock GetBlockById(string id)
        {
            TrainingBlock block = _trainingDataService.GetBlockById(id);
            IsEditMode = block != null;
            EditModeChanged?.Invoke(IsEditMode);
            _currentTraining = _trainingDataService.GetDataById(block?.TrainingId ?? id); // TODO: А НОВОЙ ТРЕНИРОВКИ ТО ТУТ НЕТУ!!
            return block ?? new TrainingBlock(_currentTraining.Id);
        }

        private void Load()
        {
            _exercises = _currentBlock?.Exercises;
            Approaches = _currentBlock?.Approaches ?? 1;
            ApproachesTimeSpan = _currentBlock?.ApproachesTimeSpan ?? new TimeSpan(0,0,0);
            Sets = _currentBlock?.Sets ?? 1;
            RestAfterApproachTimeSpan = _currentBlock?.RestAfterApproachTimeSpan ?? new TimeSpan(0,0,0);
            RestAfterSetTimeSpan = _currentBlock?.RestAfterSetTimeSpan ?? new TimeSpan(0,0,0);
            RestAfterBlockTimeSpan = _currentBlock?.RestAfterBlockTimeSpan ?? new TimeSpan(0,0,0);
            CanSaveChanged?.Invoke(CanSave);
            BlockChanged?.Invoke();
        }

        private void TrainingDataServiceOnDataUpdated(IReadOnlyList<Training> trainings)
        {
            UpdateId(BlockId);
        }

        public void RemoveExercise(string exerciseInBlockId)
        {
            _exercises.Remove(_exercises.Find(block => block.Id == exerciseInBlockId));
            CanSaveChanged?.Invoke(CanSave);
            BlockChanged?.Invoke();
        }

        public void Save()
        {
            _currentBlock.Exercises = _exercises;
            _currentBlock.Approaches = Approaches;
            _currentBlock.ApproachesTimeSpan = ApproachesTimeSpan;
            _currentBlock.Sets = Sets;
            _currentBlock.RestAfterApproachTimeSpan = RestAfterApproachTimeSpan;
            _currentBlock.RestAfterSetTimeSpan = RestAfterSetTimeSpan;
            _currentBlock.RestAfterBlockTimeSpan = RestAfterBlockTimeSpan;
            
            _currentTraining.AddOrUpdateBlock(_currentBlock);
            _trainingDataService.UpdateData(_currentTraining);
        }

        public void UpdateRepetition(string exerciseId, int repetitions)
        {
            _exercises.Find(ex => ex.Id == exerciseId).Repetitions = repetitions;
        }

        public void UpdateDuration(string exerciseId, int durationSeconds)
        {
            _exercises.Find(ex => ex.Id == exerciseId).DurationTimeSpan = 
                new TimeSpan(0,0, durationSeconds);
        }

        public void UpdateEquipmentWeight(string exerciseId, (string Id, float Weight, WeightType WeightType) weightData)
        {
            EquipmentInBlock eq = _exercises.Find(ex => ex.Id == exerciseId).EquipmentWeights
                .Find(eqW => eqW.Equipment.Equipment.Id == weightData.Id);
            eq.Weight = weightData.Weight;
            eq.WeightType = weightData.WeightType;
        }
    }
}
