using System;
using System.Collections.Generic;

public class CreateExerciseViewModel
{
    public event Action ExerciseChanged; // Для обновления UI
    public event Action<bool> CanSaveChanged; // Для активации кнопки "Сохранить"

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

    public void Load()
    {
        var data = _dataService.Load();
        AllEquipments.Clear();
        AllEquipments.AddRange(data.Equipments);
        ExerciseChanged?.Invoke();
    }

    public void UpdateEquipmentQuantity(Equipment eq, int quantity)
    {
        var existing = RequiredEquipment.Find(r => r.Equipment == eq);
        if (quantity > 0)
        {
            if (existing != null)
            {
                existing.Quantity = quantity;
            }
            else
            {
                RequiredEquipment.Add(new ExerciseEquipment { Equipment = eq, Quantity = quantity });
            }
        }
        else if (existing != null)
        {
            RequiredEquipment.Remove(existing);
        }
        ExerciseChanged?.Invoke();
    }

    public void RemoveEquipment(Equipment eq)
    {
        var existing = RequiredEquipment.Find(r => r.Equipment == eq);
        if (existing != null)
        {
            RequiredEquipment.Remove(existing);
        }
        
        var data = _dataService.Load();
        var itemToDelete = data.Equipments.Find(r=> r.Name == eq.Name);
        if (itemToDelete != null)
        {
            data.Equipments.Remove(itemToDelete);
        }
        _dataService.Save(data);
        
        ExerciseChanged?.Invoke();
    }

    public void Save()
    {
        if (string.IsNullOrEmpty(Name)) return;
        var exercise = new Exercise
        {
            Name = Name,
            Description = Description,
            RequiredEquipment = RequiredEquipment
        };
        var data = _dataService.Load();
        data.Exercises.Add(exercise);
        _dataService.Save(data);
    }

    public void Dispose()
    {
        _dataService.DataChanged -= Load;
    }
}