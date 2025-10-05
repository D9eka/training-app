using System;

public class CreateEquipmentViewModel
{
    public event Action EquipmentChanged;

    public string Name { get; set; }
    public bool HasQuantity { get; set; }
    public bool HasWeight { get; set; }

    private readonly DataService _dataService;

    public CreateEquipmentViewModel(DataService dataService)
    {
        _dataService = dataService;
    }

    public void Save()
    {
        if (string.IsNullOrEmpty(Name)) return;
        var equipment = new Equipment
        {
            Name = Name,
            HasQuantity = HasQuantity,
            HasWeight = HasWeight
        };
        var data = _dataService.Load();
        data.Equipments.Add(equipment);
        _dataService.Save(data);
        EquipmentChanged?.Invoke();
    }
}