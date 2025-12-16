using System;
using Data;
using Models;
using Screens.ViewModels;

namespace Screens.CreateEquipment
{
    public class CreateEquipmentViewModel : IViewModel
    {
        public event Action DataUpdated;
        
        private readonly IDataService<Equipment> _equipmentDataService;
        private string _name = string.Empty;

        public string Name
        {
            get => _name;
            set
            {
                _name = value ?? string.Empty;
                DataUpdated?.Invoke();
            }
        }

        public bool HasQuantity { get; set; }

        public bool HasWeight { get; set; }

        public bool CanSave => !string.IsNullOrWhiteSpace(_name);

        public CreateEquipmentViewModel(IDataService<Equipment> equipmentDataService)
        {
            _equipmentDataService = equipmentDataService;
        }

        public void Save()
        {
            if (!CanSave) return;
            Equipment eq = new Equipment(Name.Trim(), HasQuantity, HasWeight);
            _equipmentDataService.AddData(eq);
            Clear();
        }

        private void Clear()
        {
            Name =  string.Empty;
            HasQuantity = false;
            HasWeight = false;
            DataUpdated?.Invoke();
        }
    }
}