using UnityEngine;

internal interface IHealth
{
    public Transform Transform { get; }

    public float MaxHealth { get; }
    public float CurrentHealth { get; }

    public void ApplyDamage(float amount);
    public void Restore(float value);
}
