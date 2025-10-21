using System;
using System.Globalization;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class WeightTracking : IModel
    {
        [SerializeField] private DateTime _time;
        [SerializeField] private float _weight;

        public DateTime Time
        {
            get => _time;
            set => _time = value;
        }

        public float Weight
        {
            get => _weight;
            set => _weight = value;
        }

        public string Id => _time.ToString(CultureInfo.InvariantCulture);

        public WeightTracking(DateTime time, float weight)
        {
            _time = time;
            _weight = weight;
        }
    }
}