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
        private readonly IDataService<Equipment> _equipmentDataService;
    
        private Training _currentTraining;
        private TrainingBlock _currentBlock;

        public event Action<bool> EditModeChanged;
        public event Action BlockChanged;
        
        private List<ExerciseInBlock> _exercisesInBlock = new List<ExerciseInBlock>();
        
        public string BlockId { get; private set; }

        public int Approaches { get; set; } = 1;
        public int ApproachesDurationSeconds { get; set; }
        public int Sets { get; set; } = 1;
        public int RestAfterApproachSeconds { get; set; }
        public int RestAfterSetSeconds { get; set; }
        public int RestAfterBlockSeconds { get; set; }
        
        public bool CanSave => _exercisesInBlock.Count > 0;

        public CreateTrainingBlockViewModel(TrainingDataService trainingDataService, 
            IDataService<Exercise> exerciseDataService,
            IDataService<Equipment> equipmentDataService)
        {
            _trainingDataService =  trainingDataService;
            _exerciseDataService = exerciseDataService;
            _equipmentDataService = equipmentDataService;
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
                if (exercise == null)
                    continue;

                var equipments = new List<EquipmentInBlockViewData>();
                foreach (var eq in exercise.RequiredEquipment)
                {
                    Equipment equipment = _equipmentDataService.GetDataById(eq.EquipmentId);
                    EquipmentInBlock equipmentInBlock = exerciseInBlock.EquipmentWeights?
                        .Find(e => e.Equipment.EquipmentId == eq.EquipmentId);

                    equipments.Add(new EquipmentInBlockViewData
                    {
                        Id = eq.EquipmentId,
                        Name = equipment?.Name ?? "???",
                        Quantity = eq.Quantity,
                        NeedWeight = equipment?.HasWeight ?? false,
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
                    DurationSeconds = exerciseInBlock.DurationSeconds
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
            _currentBlock.ApproachesSeconds = ApproachesDurationSeconds;
            _currentBlock.Sets = Sets;
            _currentBlock.RestAfterApproachSeconds = RestAfterApproachSeconds;
            _currentBlock.RestAfterSetSeconds = RestAfterSetSeconds;
            _currentBlock.RestAfterBlockSeconds = RestAfterBlockSeconds;
            
            _currentTraining.AddOrUpdateBlock(_currentBlock);
            _trainingDataService.UpdateData(_currentTraining);
        }

        public void UpdateRepetition(string exerciseId, int repetitions)
        {
            _exercisesInBlock.Find(ex => ex.Id == exerciseId).Repetitions = repetitions;
        }

        public void UpdateDuration(string exerciseId, int durationSeconds)
        {
            _exercisesInBlock.Find(ex => ex.Id == exerciseId).DurationSeconds = durationSeconds;
        }

        public void UpdateEquipmentWeight(string exerciseId, (string Id, float Weight, WeightType WeightType) weightData)
        {
            var exercise = _exercisesInBlock.Find(ex => ex.Id == exerciseId);
            if (exercise == null) return;

            var weights = exercise.EquipmentWeights ??= new List<EquipmentInBlock>();
            var eq = weights.Find(eqW => eqW.Equipment.EquipmentId == weightData.Id);
            if (eq == null)
            {
                eq = new EquipmentInBlock(new ExerciseEquipmentRef(weightData.Id, 0));
                weights.Add(eq);
            }
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
            ApproachesDurationSeconds = _currentBlock?.ApproachesSeconds ?? 0;
            Sets = _currentBlock?.Sets ?? 1;
            RestAfterApproachSeconds = _currentBlock?.RestAfterApproachSeconds ?? 0;
            RestAfterSetSeconds = _currentBlock?.RestAfterSetSeconds ?? 0;
            RestAfterBlockSeconds = _currentBlock?.RestAfterBlockSeconds ?? 0;
            BlockChanged?.Invoke();
        }

        private void Clear()
        {
            _currentTraining = null;
            _currentBlock = null;
            _exercisesInBlock = new List<ExerciseInBlock>();
            BlockId = string.Empty;
            Approaches = 1;
            ApproachesDurationSeconds = 0;
            Sets = 1;
            RestAfterApproachSeconds = 0;
            RestAfterSetSeconds = 0;
            RestAfterBlockSeconds = 0;
            BlockChanged?.Invoke();
        }
    }
}
