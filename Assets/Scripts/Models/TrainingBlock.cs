using System;
using System.Collections.Generic;

[Serializable]
public class TrainingBlock
{
    public List<ExerciseInBlock> Exercises = new();
    public int Approaches; // Подходы
    public int Sets; // Сеты
    public float RestAfterApproach; // Секунды
    public float RestAfterSet; // Секунды
    public float RestAfterBlock; // Секунды
}