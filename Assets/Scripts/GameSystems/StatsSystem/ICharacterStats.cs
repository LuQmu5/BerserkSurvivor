using System.Collections.Generic;

public interface ICharacterStats
{
    void AddTemporaryMultiplier(StatNames statName, float multiplier, float duration);
    void CleanUpExpiredModifiers(List<StatModifier> modifiers);
    float GetCurrentStatValue(StatNames statName);
    bool TryGetCurrentValueOfStat(StatNames statName, out float result);
}