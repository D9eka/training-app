using System;
using Data;
using Models;

namespace Screens.CreateEquipment
{
    public class CreateEquipmentViewModel
    {
        private readonly IDataService _dataService;
        private string _name = string.Empty;
        private bool _hasQuantity;
        private bool _hasWeight;

        public event Action<bool> CanSaveChanged;

        public string Name
        {
            get => _name;
            set
            {
                _name = value ?? string.Empty;
                CanSaveChanged?.Invoke(CanSave);
            }
        }

        public bool HasQuantity
        {
            get => _hasQuantity;
            set
            {
                _hasQuantity = value;
                CanSaveChanged?.Invoke(CanSave);
            }
        }

        public bool HasWeight
        {
            get => _hasWeight;
            set
            {
                _hasWeight = value;
                CanSaveChanged?.Invoke(CanSave);
            }
        }

        public bool CanSave => !string.IsNullOrWhiteSpace(_name);

        public CreateEquipmentViewModel(IDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        public void Save()
        {
            if (!CanSave) return;
            Equipment eq = new Equipment(Name.Trim(), HasQuantity, HasWeight);
            _dataService.AddEquipment(eq);
        }
    }
}