internal interface IHealth
{
    public float MaxHealth { get; }
    public float CurrentHealth { get; }

    public void ApplyDamage(float amount);
}