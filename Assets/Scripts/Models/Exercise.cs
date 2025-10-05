using System;
using System.Collections.Generic;

[Serializable]
public class Exercise
{
    public string Name;
    public string Description;
    public List<ExerciseEquipment> RequiredEquipment = new();
}