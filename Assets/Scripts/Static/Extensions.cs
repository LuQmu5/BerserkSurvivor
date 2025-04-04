using System;
using UnityEngine;

public static class Extensions
{
    private static System.Random _random = new System.Random();

    public static Vector3 RandomOffset(this Transform transform, float x, float y, float z)
    {
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-x, x), UnityEngine.Random.Range(-y, y), UnityEngine.Random.Range(-z, z));

        return transform.position + randomOffset;
    }

    public static T GetRandomEnumValue<T>(this System.Random random) where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }
}