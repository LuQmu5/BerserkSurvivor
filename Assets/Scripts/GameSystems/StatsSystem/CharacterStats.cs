using DG.Tweening.Plugins.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatNames
{
    Agility = 0,
    AttackSpeed = 1,
    AttackCooldown = 2,
    MovementSpeed = 3,
    MaxHealth = 4
}

public class CharacterStats
{
    private readonly Stat[] _stats;

    public CharacterStats(StatsData data)
    {
        _stats = data.Stats;
    }

    public bool TryGetCurrentValueOfStat(StatNames statName, out float result)
    {
        result = 0;
        Stat item = _stats.FirstOrDefault(i => i.Name == statName);

        if (item.Value > 0)
            result = item.Value;

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