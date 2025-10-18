using System;
using System.Collections.Generic;
using Data;
using Models;
using Screens.ViewModels;

namespace Screens.CreateExercise
{
    public class CreateExerciseViewModel: IViewModel
    {
        public bool IsEditMode { get; private set; }
        
        private readonly IDataService<Exercise> _exerciseDataService;
        private readonly IDataService<Equipment> _equipmentDataService;
        private string _name = string.Empty;
    
        private Exercise _currentExercise;

        public event Action<bool> EditModeChanged;
        public event Action<bool> CanSaveChanged;
        public event Action EquipmentsChanged;

        public string Name
        {
            get => _name;
            set { _name = value ?? string.Empty; CanSaveChanged?.Invoke(CanSave); }
        }

        public string Description { get; set; }

        public bool CanSave => !string.IsNullOrWhiteSpace(Name);

        public IReadOnlyList<Equipment> AllEquipments { get; private set; } = new List<Equipment>();
        public List<ExerciseEquipmentRef> RequiredEquipment { get; private set; } = new List<ExerciseEquipmentRef>();

        public CreateExerciseViewModel(IDataService<Exercise> exerciseDataService, 
            IDataService<Equipment> equipmentDataService, string exerciseId)
        {
            _exerciseDataService =  exerciseDataService;
            _equipmentDataService = equipmentDataService;
            _currentExercise = GetExerciseById(exerciseId);
            LoadEquipments();
            _equipmentDataService.DataUpdated += EquipmentDataServiceOnDataUpdated;
        }

        private Exercise GetExerciseById(string exerciseId)
        {
            Exercise exercise = _exerciseDataService.GetDataById(exerciseId);
            IsEditMode = exercise != null;
            EditModeChanged?.Invoke(IsEditMode);
            if (exercise == null) 
                return new Exercise();
            
            Name = _currentExercise.Name;
            Description = _currentExercise.Description;
            foreach (var equipmentRef in _currentExercise.RequiredEquipment)
            {
                RequiredEquipment.Add(equipmentRef);
            }
            return exercise;
        }

        private void LoadEquipments()
        {
            AllEquipments = _equipmentDataService.Cache;
            EquipmentsChanged?.Invoke();
        }

        private void EquipmentDataServiceOnDataUpdated(IReadOnlyList<Equipment> cache)
        {
            EquipmentsChanged?.Invoke();
        }

        public void UpdateEquipmentQuantity(Equipment eq, int quantity)
        {
            if (eq == null) return;
            var existing = RequiredEquipment.Find(r => r.EquipmentId == eq.Id);
            if (quantity <= 0)
            {
                if (existing != null) RequiredEquipment.Remove(existing);
            }
            else
            {
                if (existing != null) existing.Quantity = quantity;
                else RequiredEquipment.Add(new ExerciseEquipmentRef(eq.Id, quantity));
            }
            EquipmentsChanged?.Invoke();
        }

        public void RemoveEquipment(Equipment eq)
        {
            if (eq == null) return;
            RequiredEquipment.RemoveAll(r => r.EquipmentId == eq.Id);
            _equipmentDataService.RemoveData(eq.Id);
        }

        public void Save()
        {
            if (!CanSave) return;
            _currentExercise.Name = Name;
            _currentExercise.Description = Description;
            foreach (var r in RequiredEquipment)
                _currentExercise.AddOrUpdateEquipment(r.EquipmentId, r.Quantity);

            if (IsEditMode)
            {
                _exerciseDataService.UpdateData(_currentExercise);
            }
            else
            {
                _exerciseDataService.AddData(_currentExercise);
            }
        }
    }
}
