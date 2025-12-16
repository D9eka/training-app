using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Screens.ViewModels;

namespace Screens.WeightTracker
{
    public class WeightTrackerViewModel : IViewModel
    {
        private readonly IDataService<WeightTracking> _weightTrackingDataService;
        
        public IReadOnlyList<WeightItemViewData> Weights { get; private set; }
        public List<float> GraphValues { get; private set; } = new List<float>();
        
        public event Action WeightsChanged;
        public event Action GraphDataChanged;

        public WeightTrackerViewModel(IDataService<WeightTracking> weightTrackingDataService)
        {
            _weightTrackingDataService = weightTrackingDataService;
            
            Load(_weightTrackingDataService.Cache);
            _weightTrackingDataService.DataUpdated += Load;
            UpdateGraphData(GraphMode.Weekly);
        }

        private void Load(IReadOnlyList<WeightTracking> allWeights)
        {
            if (allWeights.Count == 0)
            {
                Weights = new List<WeightItemViewData>();
                return;
            }
            
            List<WeightTracking> weights = allWeights.OrderByDescending(w => w.Time).ToList();
            List<WeightItemViewData> weightsView = new List<WeightItemViewData>();
            for (int i = 0; i < weights.Count; i++)
            {
                weightsView.Add(new WeightItemViewData(weights[i], i < weights.Count - 1 ? weights[i + 1] : null));
            }
            Weights = weightsView;
            WeightsChanged?.Invoke();
        }

        public void UpdateGraphData(GraphMode mode)
        {
            IReadOnlyList<WeightTracking> weightData = _weightTrackingDataService.Cache;
            GraphValues = new List<float>();

            if (weightData == null || weightData.Count == 0)
                return;

            IEnumerable<WeightTracking> filtered;

            if (mode == GraphMode.All)
            {
                filtered = weightData;
            }
            else
            {
                int days = mode switch
                {
                    GraphMode.Weekly => 7,
                    GraphMode.Monthly => 30,
                    GraphMode.Yearly => 365,
                    _ => 7
                };

                DateTime fromDate = DateTime.Now.Date.AddDays(-(days - 1));

                filtered = weightData.Where(w => w.Time >= fromDate);
            }

            GraphValues = filtered
                .Select(w => w.Weight)
                .ToList();

            GraphDataChanged?.Invoke();
        }
    }
}