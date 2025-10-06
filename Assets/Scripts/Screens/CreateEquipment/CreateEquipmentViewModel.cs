using System;
using Data;
using Models;

namespace Screens.CreateEquipment
{
    public class CreateEquipmentViewModel
    {
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
        public bool HasQuantity { get; set; }
        public bool HasWeight { get; set; }

        private readonly DataService _dataService;
        private string _name;

        public CreateEquipmentViewModel(DataService dataService)
        {
            _dataService = dataService;
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(Name)) return;
            Equipment equipment = new Equipment(Name, HasQuantity, HasWeight);
            AppData data = _dataService.Load();
            data.Equipments.Add(equipment);
            _dataService.Save(data);
        }
    }
}