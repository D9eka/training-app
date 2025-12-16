using System;
using Data;
using Models;
using Screens.ViewModels;

namespace Screens.AddWeight
{
    public class AddWeightViewModel : IViewModel
    {
        public event Action DataUpdated;
        
        private readonly IDataService<WeightTracking> _weightDataService;
        private float _weight;

        public float Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                DataUpdated?.Invoke();
            }
        }

        public bool CanSave => _weight != 0;
        
        public AddWeightViewModel(IDataService<WeightTracking> weightDataService)
        {
            _weightDataService = weightDataService;
        }
        
        public void Save()
        {
            if (!CanSave) return;
            WeightTracking weightTracking = new WeightTracking(DateTime.Now, _weight);
            _weightDataService.AddData(weightTracking);
            Clear();
        }

        private void Clear()
        {
            Weight = 0;
            DataUpdated?.Invoke();
        }
    }
}