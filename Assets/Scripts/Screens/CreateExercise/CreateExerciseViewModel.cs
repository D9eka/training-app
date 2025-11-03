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
            UpdateId(exerciseId);
            _equipmentDataService.DataUpdated += EquipmentDataServiceOnDataUpdated;
        }

        public void UpdateId(string exerciseId)
        {
            _currentExercise = GetExerciseById(exerciseId);
            RequiredEquipment = _currentExercise.RequiredEquipment;
            Load();
        }

        private Exercise GetExerciseById(string exerciseId)
        {
            Exercise exercise = _exerciseDataService.GetDataById(exerciseId);
            IsEditMode = exercise != null;
            EditModeChanged?.Invoke(IsEditMode);
            return exercise ?? new Exercise();
        }

        private void Load()
        {
            Name = _currentExercise.Name;
            Description = _currentExercise.Description;
            LoadEquipments();
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
            var existing = RequiredEquipment.Find(r => r.Equipment == eq);
            if (quantity <= 0)
            {
                if (existing != null) RequiredEquipment.Remove(existing);
            }
            else
            {
                if (existing != null) existing.Quantity = quantity;
                else RequiredEquipment.Add(new ExerciseEquipmentRef(eq, quantity));
            }
            EquipmentsChanged?.Invoke();
        }

        public void RemoveEquipment(Equipment eq)
        {
            if (eq == null) return;
            RequiredEquipment.RemoveAll(r => r.Equipment == eq);
            _equipmentDataService.RemoveData(eq.Id);
        }

        public void Save()
        {
            if (!CanSave) return;
            _currentExercise.Name = Name;
            _currentExercise.Description = Description;
            foreach (var r in RequiredEquipment.ToArray())
                _currentExercise.AddOrUpdateEquipment(r.Equipment, r.Quantity);

            if (IsEditMode)
            {
                _exerciseDataService.UpdateData(_currentExercise);
            }
            else
            {
                _exerciseDataService.AddData(_currentExercise);
            }
        }

        public int GetQuantity(string eqId)
        {
            return RequiredEquipment.Find(r => r.Equipment.Id == eqId)?.Quantity ?? 0;
        }
    }
}
