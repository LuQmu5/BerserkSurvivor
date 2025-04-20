
using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatNames
{
    Agility = 0,
    AttackSpeed = 1,
    AttackCooldown = 2,
    MovementSpeed = 3,
    MaxHealth = 4
}

public class CharacterStats : ICharacterStats
{
    private readonly Dictionary<StatNames, Stat> _baseStats = new();
    private readonly Dictionary<StatNames, List<StatModifier>> _modifiers = new();

    public CharacterStats(StatsData data)
    {
        foreach (Stat stat in data.Stats)
        {
            _baseStats.Add(stat.Name, stat);
        }
    }

    public float GetCurrentStatValue(StatNames statName)
    {
        if (!_baseStats.TryGetValue(statName, out Stat baseStat))
            return 0;

        float baseValue = baseStat.Value;
        float multiplier = 1f;

        if (_modifiers.TryGetValue(statName, out var modList))
        {
            CleanUpExpiredModifiers(modList);

            foreach (var mod in modList)
            {
                multiplier *= mod.Multiplier;
            }
        }

        return baseValue * multiplier;
    }

    public void AddTemporaryMultiplier(StatNames statName, float multiplier, float duration)
    {
        if (!_modifiers.ContainsKey(statName))
            _modifiers[statName] = new List<StatModifier>();

        var modifier = new StatModifier(multiplier, Time.time + duration);
        _modifiers[statName].Add(modifier);
    }

    public void CleanUpExpiredModifiers(List<StatModifier> modifiers)
    {
        modifiers.RemoveAll(mod => Time.time > mod.EndTime);
    }

    public bool TryGetCurrentValueOfStat(StatNames statName, out float result)
    {
        result = GetCurrentStatValue(statName);
        return result > 0;
    }
}

[Serializable]
public struct Stat
{
    [field:SerializeField] public StatNames Name { get; private set; }
    [field: SerializeField] public float Value { get; private set; }

    public Stat(StatNames statName, float value)
    {
        Name = statName;
        Value = value;
    }

    public bool TryChangeValue(float newValue)
    {
        if (newValue < 0)
            return false;

        Value = newValue;

        return true;
    }
}

public struct StatModifier
{
    public float Multiplier;
    public float EndTime;

    public StatModifier(float multiplier, float endTime)
    {
        Multiplier = multiplier;
        EndTime = endTime;
    }
}