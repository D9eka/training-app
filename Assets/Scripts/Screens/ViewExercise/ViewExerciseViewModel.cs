using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.ViewTrainings;

namespace Screens.ViewExercise
{
    public class ViewExerciseViewModel : IUpdatableViewModel<ExerciseIdParameter>
    {
        private readonly IDataService<Training> _trainingDataService;
        private readonly IDataService<Exercise> _exerciseDataService;
        private readonly IDataService<Equipment> _equipmentDataService;

        private string _exerciseId;
        private Exercise _currentExercise;

        public string ExerciseId
        {
            get => _exerciseId;
            set
            {
                if (_exerciseId == value) return;
                _exerciseId = value;
                Load();
            }
        }
        public string ExerciseName { get; private set; }
        public string ExerciseDescription { get; private set; }
        public string EquipmentsText { get; private set; }
        public bool IsNotFound { get; private set; }
        public List<TrainingViewData> UsingInTrainings { get; private set; } = new List<TrainingViewData>();

        public event Action ExerciseChanged;

        public ViewExerciseViewModel(IDataService<Training> trainingDataService, 
            IDataService<Exercise> exerciseDataService, IDataService<Equipment> equipmentDataService, 
            ExerciseIdParameter param)
        {
            _trainingDataService = trainingDataService;
            _exerciseDataService = exerciseDataService;
            _equipmentDataService = equipmentDataService;
            UpdateParameter(param);
            _exerciseDataService.DataUpdated += _ => Load();
            Load();
        }

        public void UpdateParameter(ExerciseIdParameter param)
        {
            ExerciseId = param.ExerciseId;
        }

        private void Load()
        {
            _currentExercise = _exerciseDataService.GetDataById(ExerciseId);
            UpdateUiData();
            ExerciseChanged?.Invoke();
        }

        private void UpdateUiData()
        {
            if (_currentExercise == null)
            {
                Clear();
                return;
            }

            IsNotFound = false;
            ExerciseName = _currentExercise.Name;
            ExerciseDescription = $"Описание: {_currentExercise.Description}";

            EquipmentsText = GetEquipmentsText();
            UsingInTrainings = GetUsingIsTrainings();
        }

        public void DeleteExercise()
        {
            _exerciseDataService.RemoveData(ExerciseId);
        }

        private void Clear()
        {
            IsNotFound = true;
            ExerciseName = "Упражнение не найдено";
            ExerciseDescription = "";
            EquipmentsText = "";
            UsingInTrainings = new List<TrainingViewData>();
        }

        private string GetEquipmentsText()
        {
            if (_currentExercise.RequiredEquipment == null || _currentExercise.RequiredEquipment.Count == 0)
            {
                return "Без оборудования";
            }

            var equipmentList = _currentExercise.RequiredEquipment.Select(r =>
            {
                Equipment eq = _equipmentDataService.GetDataById(r.EquipmentId);
                return $"{(eq?.Name ?? "??")} x{r.Quantity}";
            });

            return $"Нужно: {string.Join(", ", equipmentList)}";
        }

        private List<TrainingViewData> GetUsingIsTrainings()
        {
            return _trainingDataService.Cache
                .Where(training =>
                    training.Blocks.Any(block =>
                        block.Exercises.Any(ex => ex.ExerciseId == ExerciseId)
                    )
                )
                .Select(training => new TrainingViewData(training))
                .ToList();
        }
    }
}
