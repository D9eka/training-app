using System;
using System.Globalization;

namespace Models
{
    [Serializable]
    public class WeightTracking : IModel
    {
        public DateTime Time;
        public float Weight;
        
        public string Id => Time.ToString(CultureInfo.InvariantCulture);

        public WeightTracking(DateTime time, float weight)
        {
            Time = time;
            Weight = weight;
        }
    }
}