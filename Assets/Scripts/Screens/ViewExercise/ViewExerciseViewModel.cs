using System;
using System.Linq;
using Data;
using Models;
using Screens.ViewModels;

namespace Screens.ViewExercise
{
    public class ViewExerciseViewModel: IViewModel
    {
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

        public event Action ExerciseChanged;

        public ViewExerciseViewModel(
            IDataService<Exercise> exerciseDataService,
            IDataService<Equipment> equipmentDataService,
            string exerciseId)
        {
            _exerciseDataService = exerciseDataService ?? throw new ArgumentNullException(nameof(exerciseDataService));
            _equipmentDataService = equipmentDataService ?? throw new ArgumentNullException(nameof(equipmentDataService));
            ExerciseId = exerciseId ?? throw new ArgumentException("Exercise ID cannot be null or empty", nameof(exerciseId));

            _exerciseDataService.DataUpdated += _ => Load();
            Load();
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
                IsNotFound = true;
                ExerciseName = "Упражнение не найдено";
                ExerciseDescription = "";
                EquipmentsText = "";
                return;
            }

            IsNotFound = false;
            ExerciseName = _currentExercise.Name;
            ExerciseDescription = $"Описание: {_currentExercise.Description}";

            if (_currentExercise.RequiredEquipment == null || _currentExercise.RequiredEquipment.Count == 0)
            {
                EquipmentsText = "Без оборудования";
                return;
            }

            var equipmentList = _currentExercise.RequiredEquipment.Select(r =>
            {
                var eq = _equipmentDataService.GetDataById(r.EquipmentId);
                return $"{(eq?.Name ?? "??")} x{r.Quantity}";
            });

            EquipmentsText = $"Нужно: {string.Join(", ", equipmentList)}";
        }

        public void DeleteExercise()
        {
            _exerciseDataService.RemoveData(ExerciseId);
        }
    }
}
