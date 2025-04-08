using System;
using System.Linq;
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
        if (obj is MagicElementsPattern other)
        {
            var thisElements = new[] { FirstSigil, SecondSigil, ThirdSigil }.OrderBy(x => x).ToArray();
            var otherElements = new[] { other.FirstSigil, other.SecondSigil, other.ThirdSigil }.OrderBy(x => x).ToArray();

            return thisElements.SequenceEqual(otherElements);
        }
        return false;
    }

    public override int GetHashCode()
    {
        var sortedElements = new[] { FirstSigil, SecondSigil, ThirdSigil }.OrderBy(x => x).ToArray();
        return HashCode.Combine(sortedElements[0], sortedElements[1], sortedElements[2]);
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
