using System;
using System.Collections.Generic;
using Data;
using Models;

namespace Screens.CreateExercise
{
    public class CreateExerciseViewModel
    {
        public bool IsEditMode { get; private set; }
        
        private readonly IDataService _dataService;
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

        public CreateExerciseViewModel(IDataService dataService, string exerciseId)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            FindExerciseById(exerciseId);
            LoadEquipments();
            _dataService.EquipmentsUpdated += LoadEquipments;
        }

        private void FindExerciseById(string exerciseId)
        {
            _currentExercise = _dataService.GetExerciseById(exerciseId);
            IsEditMode = _currentExercise != null;
            EditModeChanged?.Invoke(IsEditMode);
            if (_currentExercise == null) return;
            
            Name = _currentExercise.Name;
            Description = _currentExercise.Description;
            foreach (var equipmentRef in _currentExercise.RequiredEquipment)
            {
                RequiredEquipment.Add(equipmentRef);
            }
        }

        private void LoadEquipments()
        {
            AllEquipments = _dataService.GetAllEquipments();
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
            _dataService.RemoveEquipment(eq.Id);
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
                _dataService.UpdateExercise(_currentExercise);
            }
            else
            {
                _dataService.AddExercise(_currentExercise);
            }
        }
    }
}
