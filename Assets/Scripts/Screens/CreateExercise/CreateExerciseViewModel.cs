using System;
using System.Collections.Generic;
using Data;
using Models;

namespace Screens.CreateExercise
{
    public class CreateExerciseViewModel
    {
        public event Action ExerciseChanged;
        public event Action<bool> CanSaveChanged;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                CanSaveChanged?.Invoke(!string.IsNullOrWhiteSpace(_name));
            }
        }
        public string Description { get; set; }
        public List<ExerciseEquipment> RequiredEquipment { get; } = new List<ExerciseEquipment>();
        public List<Equipment> AllEquipments { get; private set; } = new List<Equipment>();

        private readonly DataService _dataService;
        private string _name;

        public CreateExerciseViewModel(DataService dataService)
        {
            _dataService = dataService;
            _dataService.DataChanged += Load;
            Load();
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(Name)) return;
            Exercise exercise = new Exercise(Name, Description, RequiredEquipment);
            AppData data = _dataService.Load();
            data.Exercises.Add(exercise);
            _dataService.Save(data);
        }

        public void Load()
        {
            AppData data = _dataService.Load();
            AllEquipments.Clear();
            AllEquipments.AddRange(data.Equipments);
            ExerciseChanged?.Invoke();
        }

        public void UpdateEquipmentQuantity(Equipment eq, int quantity)
        {
            ExerciseEquipment existing = RequiredEquipment.Find(r => r.Equipment == eq);
            if (quantity > 0)
            {
                if (existing != null)
                {
                    existing.Quantity = quantity;
                }
                else
                {
                    RequiredEquipment.Add(new ExerciseEquipment(eq, quantity));
                }
            }
            else if (existing != null)
            {
                RequiredEquipment.Remove(existing);
            }
            ExerciseChanged?.Invoke();
        }
        
        public void AddEquipmentToExercise(Exercise exercise, Equipment equipment, int quantity)
        {
            AppData data = _dataService.Load();
            int oldExerciseIndex = data.Exercises.FindIndex(e => e.Id == exercise.Id);
            
            exercise.RequiredEquipment.Add(new ExerciseEquipment(equipment, quantity));
            data.Exercises[oldExerciseIndex] = exercise;
            
            foreach (Training training in data.Trainings)
            {
                foreach (TrainingBlock block in training.Blocks)
                {
                    List<ExerciseInBlock> exercisesToEdit = block.Exercises.FindAll(e => e.Exercise.Id == exercise.Id);
                    if (exercisesToEdit.Count == 0) 
                        continue;
                    foreach (ExerciseInBlock exerciseToEdit in exercisesToEdit)
                    {
                        exerciseToEdit.EquipmentWeights.Add(new EquipmentInBlock(equipment));
                    }
                }
            }
            
            ExerciseChanged?.Invoke();
        }

        public void RemoveEquipment(Equipment eq)
        {
            AppData data = _dataService.Load();
            foreach (Exercise exercise in data.Exercises)
            {
                ExerciseEquipment existing = exercise.RequiredEquipment.Find(r => r.Equipment.Id == eq.Id);
                if (existing == null) 
                    continue;
                
                RequiredEquipment.Remove(existing);
                foreach (Training training in data.Trainings)
                {
                    foreach (TrainingBlock block in training.Blocks)
                    {
                        foreach (ExerciseInBlock exerciseInBlock in block.Exercises)
                        {
                            if (exerciseInBlock.Exercise.Id != exercise.Id) 
                                continue;
                            EquipmentInBlock exerciseToDelete = exerciseInBlock.EquipmentWeights.Find(r => r.Equipment.Id == eq.Id);
                            if (exerciseToDelete != null)
                            {
                                exerciseInBlock.EquipmentWeights.Remove(exerciseToDelete);
                            }
                        }
                    }
                }
            }
        
            Equipment itemToDelete = data.Equipments.Find(r=> r.Id == eq.Id);
            if (itemToDelete != null)
            {
                data.Equipments.Remove(itemToDelete);
            }
            _dataService.Save(data);
        
            ExerciseChanged?.Invoke();
        }

        public void Dispose()
        {
            _dataService.DataChanged -= Load;
        }
    }
}