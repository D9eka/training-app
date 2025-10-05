using System;
using System.Collections.Generic;

[Serializable]
public class AppData // Корневой класс для всего
{
    public List<Equipment> Equipments = new();
    public List<Exercise> Exercises = new();
    public List<Training> Trainings = new();
    public List<WeightTracking> WeightTrackings = new();
}