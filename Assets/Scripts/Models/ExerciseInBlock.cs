using System;

[Serializable]
public class ExerciseInBlock
{
    public Exercise Exercise;
    public float Weight; // В кг или % (храните как float, тип укажите отдельно)
    public string WeightType; // "kg" или "%"
    public int Repetitions;
    public float Duration; // В секундах
}