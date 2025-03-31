using System;
using UnityEngine;

[Serializable]
public struct MagicElementsPattern
{
    [field: SerializeField] public MagicElements First { get; private set; }
    [field: SerializeField] public MagicElements Second { get; private set; }
    [field: SerializeField] public MagicElements Third { get; private set; }

    public MagicElementsPattern(MagicElements first, MagicElements second, MagicElements third)
    {
        First = first;
        Second = second;
        Third = third;
    }

    public override string ToString()
    {
        return First.ToString() + Second.ToString() + Third.ToString();
    }

    public override bool Equals(object obj)
    {
        return obj.ToString() == ToString();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(First, Second, Third);
    }

    public static bool operator ==(MagicElementsPattern c1, MagicElementsPattern c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(MagicElementsPattern c1, MagicElementsPattern c2)
    {
        return !c1.Equals(c2);
    }
}