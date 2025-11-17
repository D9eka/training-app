using System;
using System.Linq;
using Data;
using Models;
using Screens.Factories.Parameters;

namespace Screens.ViewExercise
{
    public class ViewExerciseViewModel : IUpdatableViewModel<ExerciseIdParameter>
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
            ExerciseIdParameter param)
        {
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
                Equipment eq = _equipmentDataService.GetDataById(r.EquipmentId);
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
