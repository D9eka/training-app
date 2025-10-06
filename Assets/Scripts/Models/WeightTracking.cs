using System;

namespace Models
{
    [Serializable]
    public class WeightTracking
    {
        public DateTime Time;
        public float Weight;

        public WeightTracking(DateTime time, float weight)
        {
            Time = time;
            Weight = weight;
        }
    }
}