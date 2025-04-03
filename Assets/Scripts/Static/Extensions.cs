using System;
using UnityEngine;

public static class Extensions
{
    public static Vector3 RandomOffset(this Transform transform, float x, float y, float z)
    {
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-x, x), UnityEngine.Random.Range(-y, y), UnityEngine.Random.Range(-z, z));

        return transform.position + randomOffset;
    }

    public static ItemType GetRandomItemType(this System.Random random)
    {
        Array types = Enum.GetValues(typeof(ItemType));

        return (ItemType)types.GetValue(random.Next(0, types.Length));
    }
}