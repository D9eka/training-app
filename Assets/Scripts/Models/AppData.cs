using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class AppData
    {
        public List<Equipment> Equipments = new();
        public List<Exercise> Exercises = new();
        public List<Training> Trainings = new();
    }
}