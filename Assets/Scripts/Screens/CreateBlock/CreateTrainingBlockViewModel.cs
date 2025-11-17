using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.ViewModels;

namespace Screens.CreateBlock
{
    public class CreateTrainingBlockViewModel : IUpdatableViewModel<CreateTrainingBlockParameter>
    {
        public bool IsEditMode { get; private set; }
        
        private readonly TrainingDataService _trainingDataService;
        private readonly IDataService<Exercise> _exerciseDataService;
    
        private Training _currentTraining;
        private TrainingBlock _currentBlock;

        public event Action<bool> EditModeChanged;
        public event Action BlockChanged;
        
        private List<ExerciseInBlock> _exercisesInBlock = new List<ExerciseInBlock>();
        
        public string BlockId { get; private set; }

        public int Approaches { get; set; } = 1;
        public TimeSpan ApproachesTimeSpan { get; set; }
        public int Sets { get; set; } = 1;
        public TimeSpan RestAfterApproachTimeSpan { get; set; }
        public TimeSpan RestAfterSetTimeSpan { get; set; }
        public TimeSpan RestAfterBlockTimeSpan { get; set; }
        
        public bool CanSave => _exercisesInBlock.Count > 0;

        public CreateTrainingBlockViewModel(TrainingDataService trainingDataService, 
            IDataService<Exercise> exerciseDataService, CreateTrainingBlockParameter param)
        {
            _trainingDataService =  trainingDataService;
            _exerciseDataService = exerciseDataService;
            UpdateParameter(param);
            _trainingDataService.DataUpdated += TrainingDataServiceOnDataUpdated;
        }

        public void UpdateParameter(CreateTrainingBlockParameter param)
        {
            _currentTraining = _trainingDataService.GetDataById(param.TrainingId);
            _currentBlock = param.HasBlockId ? 
                _trainingDataService.GetBlockById(param.BlockId) : new TrainingBlock(_currentTraining.Id);
            IsEditMode = param.HasBlockId;
            EditModeChanged?.Invoke(IsEditMode);
            BlockId = _currentBlock.Id;
            Load();
        }
        
        public IReadOnlyList<ExerciseInBlockViewData> GetExercises()
        {
            List<ExerciseInBlockViewData> exercises = new List<ExerciseInBlockViewData>();
            
            foreach (var exerciseInBlock in _exercisesInBlock)
            {
                Exercise exercise = _exerciseDataService.GetDataById(exerciseInBlock.ExerciseId);

                var equipments = new List<EquipmentInBlockViewData>();
                foreach (var eq in exercise.RequiredEquipment)
                {
                    EquipmentInBlock equipmentInBlock = exerciseInBlock.EquipmentWeights?
                        .Find(e => e.Equipment.Equipment.Id == eq.Equipment.Id);

                    equipments.Add(new EquipmentInBlockViewData
                    {
                        Id = eq.Equipment.Id,
                        Name = eq.Equipment.Name,
                        Quantity = eq.Quantity,
                        NeedWeight = eq.Equipment.HasWeight,
                        Weight = equipmentInBlock != null ? (int)equipmentInBlock.Weight : 0,
                        WeightType = equipmentInBlock != null ? equipmentInBlock.WeightType : WeightType.Kg
                    });
                }

                exercises.Add(new ExerciseInBlockViewData
                {
                    Id = exerciseInBlock.Id,
                    Name = exercise.Name,
                    Equipments = equipments,
                    Repetitions = exerciseInBlock.Repetitions,
                    DurationSeconds = exerciseInBlock.DurationTimeSpan.Seconds
                });
            }
            return exercises;
        }

        public int GetExerciseIndex(string exerciseInBlockId)
        {
            return _exercisesInBlock.FindIndex(block => block.Id == exerciseInBlockId);
        }

        public void RemoveExercise(string exerciseInBlockId)
        {
            _exercisesInBlock.Remove(_exercisesInBlock.Find(block => block.Id == exerciseInBlockId));
            BlockChanged?.Invoke();
        }

        public void Save()
        {
            _currentBlock.Exercises = _exercisesInBlock;
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
            _exercisesInBlock.Find(ex => ex.Id == exerciseId).Repetitions = repetitions;
        }

        public void UpdateDuration(string exerciseId, int durationSeconds)
        {
            _exercisesInBlock.Find(ex => ex.Id == exerciseId).DurationTimeSpan = 
                new TimeSpan(0,0, durationSeconds);
        }

        public void UpdateEquipmentWeight(string exerciseId, (string Id, float Weight, WeightType WeightType) weightData)
        {
            EquipmentInBlock eq = _exercisesInBlock.Find(ex => ex.Id == exerciseId).EquipmentWeights
                .Find(eqW => eqW.Equipment.Equipment.Id == weightData.Id);
            eq.Weight = weightData.Weight;
            eq.WeightType = weightData.WeightType;
        }

        public void OnCreate()
        {
            Save();
            Clear();
        }

        private void TrainingDataServiceOnDataUpdated(IReadOnlyList<Training> trainings)
        {
            if (_currentTraining != null)
            {
                UpdateParameter(new CreateTrainingBlockParameter(_currentTraining.Id, _currentBlock.Id));
            }
        }

        private void Load()
        {
            _exercisesInBlock = _currentBlock?.Exercises;
            Approaches = _currentBlock?.Approaches ?? 1;
            ApproachesTimeSpan = _currentBlock?.ApproachesTimeSpan ?? new TimeSpan(0,0,0);
            Sets = _currentBlock?.Sets ?? 1;
            RestAfterApproachTimeSpan = _currentBlock?.RestAfterApproachTimeSpan ?? new TimeSpan(0,0,0);
            RestAfterSetTimeSpan = _currentBlock?.RestAfterSetTimeSpan ?? new TimeSpan(0,0,0);
            RestAfterBlockTimeSpan = _currentBlock?.RestAfterBlockTimeSpan ?? new TimeSpan(0,0,0);
            BlockChanged?.Invoke();
        }

        private void Clear()
        {
            _currentTraining = null;
            _currentBlock = null;
            _exercisesInBlock = new List<ExerciseInBlock>();
            BlockId = string.Empty;
            Approaches = 1;
            ApproachesTimeSpan = TimeSpan.Zero;
            Sets = 1;
            RestAfterApproachTimeSpan = TimeSpan.Zero;
            RestAfterSetTimeSpan = TimeSpan.Zero;
            RestAfterBlockTimeSpan = TimeSpan.Zero;
            BlockChanged?.Invoke();
        }
    }
}
