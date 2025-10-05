using System;
using System.Collections.Generic;

[Serializable]
public class Training
{
    public string Name;
    public string Description;
    public float PrepTime; // Секунды
    public List<TrainingBlock> Blocks = new();
}