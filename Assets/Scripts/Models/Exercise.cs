using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class Exercise
    {
        public string Id;
        public string Name;
        public string Description;
        public List<ExerciseEquipment> RequiredEquipment = new();

        public Exercise(string name, string description, List<ExerciseEquipment> requiredEquipment)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
            RequiredEquipment = requiredEquipment;
        }
    }
}