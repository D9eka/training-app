using System;
using System.Collections.Generic;
using Data;
using Models;
using Screens.Factories.Parameters;
using Screens.ViewModels;

namespace Screens.CreateExercise
{
    public class CreateExerciseViewModel: IUpdatableViewModel<ExerciseIdParameter>
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
            IDataService<Equipment> equipmentDataService, ExerciseIdParameter param)
        {
            _exerciseDataService =  exerciseDataService;
            _equipmentDataService = equipmentDataService;
            UpdateParameter(param);
            _equipmentDataService.DataUpdated += EquipmentDataServiceOnDataUpdated;
        }

        public void UpdateParameter(ExerciseIdParameter param)
        {
            IsEditMode = param != null;
            EditModeChanged?.Invoke(IsEditMode);
            _currentExercise = param != null ? _exerciseDataService.GetDataById(param.ExerciseId) : new Exercise();
            Load();
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
                else RequiredEquipment.Add(new ExerciseEquipmentRef(eq, quantity));
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
            foreach (var r in RequiredEquipment.ToArray())
                _currentExercise.AddOrUpdateEquipment(r.EquipmentId, r.Quantity);
            if (IsEditMode)
            {
                _exerciseDataService.UpdateData(_currentExercise);
            }
            else
            {
                _exerciseDataService.AddData(_currentExercise);
            }

            Clear();
        }

        public int GetQuantity(string eqId)
        {
            return RequiredEquipment.Find(r => r.EquipmentId == eqId)?.Quantity ?? 0;
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
            RequiredEquipment = _currentExercise.RequiredEquipment;
            EquipmentsChanged?.Invoke();
        }

        private void EquipmentDataServiceOnDataUpdated(IReadOnlyList<Equipment> cache)
        {
            EquipmentsChanged?.Invoke();
        }

        private void Clear()
        {
            _currentExercise = null;
            Name = string.Empty;
            Description = string.Empty;
            AllEquipments = new List<Equipment>();
            RequiredEquipment = new List<ExerciseEquipmentRef>();
        }
    }
}
