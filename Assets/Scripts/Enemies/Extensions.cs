using UnityEngine;

public static class Extensions
{
    public static Vector3 RandomOffset(this Transform transform, float x, float y, float z)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-x, x), Random.Range(-y, y), Random.Range(-z, z));

        return transform.position + randomOffset;
    }
}