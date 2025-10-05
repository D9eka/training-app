using System;

[Serializable]
public class Equipment
{
    public string Name;
    public bool HasQuantity;
    public bool HasWeight;

    protected bool Equals(Equipment other)
    {
        return Name == other.Name && HasQuantity == other.HasQuantity && HasWeight == other.HasWeight;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, HasQuantity, HasWeight);
    }
}