public interface ILevelable
{
    public float MaxLevel { get; }
    public float CurrentLevel { get; }
    public float CurrentExpirience { get; }
    public float ExpirienceForNextLevel { get; }

    public void AddExpirience(float amount);
    public void OnLevelUp();
}