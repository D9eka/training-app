using Models;
using Views.Components;

namespace Screens.WeightTracker
{
    public class WeightItemViewData
    {
        public string Date { get; private set; }
        public float Weight { get; private set; }
        public float WeightDifference { get; private set; }
        public bool ShowDifference { get; private set; }

        public WeightItemViewData(WeightTracking weightItem, WeightTracking previousWeightItem = null)
        {
            Date = weightItem.Time.ToShortDateString();
            Weight = weightItem.Weight;
            ShowDifference = previousWeightItem != null;
            if (previousWeightItem != null)
            {
                WeightDifference = weightItem.Weight - previousWeightItem.Weight;
            }
        }
    }
}