using System;
using System.Globalization;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class WeightTracking : IModel
    {
        [field: SerializeField] public long TimeTicks { get; set; }
        [field: SerializeField] public float Weight { get; set; }

        public DateTime Time => new(TimeTicks);
        public string Id => Time.ToString(CultureInfo.InvariantCulture);

        public WeightTracking(DateTime time, float weight)
        {
            TimeTicks = time.Ticks;
            Weight = weight;
        }
    }
}
