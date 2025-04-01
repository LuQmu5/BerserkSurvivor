using System;
using UnityEngine;

[Serializable]
public struct MagicElementsPattern
{
    [field: SerializeField] public MagicElements FirstSigil { get; private set; }
    [field: SerializeField] public MagicElements SecondSigil { get; private set; }
    [field: SerializeField] public MagicElements ThirdSigil { get; private set; }

    public MagicElementsPattern(MagicElements firstSigil, MagicElements secondSigil, MagicElements thirdSigil)
    {
        FirstSigil = firstSigil;
        SecondSigil = secondSigil;
        ThirdSigil = thirdSigil;
    }

    public override string ToString()
    {
        return FirstSigil.ToString() + SecondSigil.ToString() + ThirdSigil.ToString();
    }

    public override bool Equals(object obj)
    {
        return obj.ToString() == ToString();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstSigil, SecondSigil, ThirdSigil);
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