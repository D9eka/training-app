using System;
using System.Globalization;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class WeightTracking : IModel
    {
        [field: SerializeField] public DateTime Time { get; set; }
        [field: SerializeField] public float Weight { get; set; }

        public string Id => Time.ToString(CultureInfo.InvariantCulture);

        public WeightTracking(DateTime time, float weight)
        {
            Time = time;
            Weight = weight;
        }
    }
}