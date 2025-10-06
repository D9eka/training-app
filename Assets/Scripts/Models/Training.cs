using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class Training
    {
        public string Id;
        public string Name;
        public string Description;
        public float PrepTimeSeconds;
        public List<TrainingBlock> Blocks = new();

        public Training(string name, string description, float prepTimeSeconds)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
            PrepTimeSeconds = prepTimeSeconds;
        }
    }
}