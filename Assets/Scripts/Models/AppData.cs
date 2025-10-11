using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class AppData
    {
        public List<Equipment> Equipments;
        public List<Exercise> Exercises;

        public AppData()
        {
            Equipments = new List<Equipment>();
            Exercises = new List<Exercise>();
        }
    }
}