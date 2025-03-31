using System;
using System.Collections.Generic;

public enum StatNames
{
    Agility = 0,
    AttackSpeed = 1,
    AttackCooldown = 2
}

public class CharacterStats
{
    private readonly Dictionary<StatNames, float> _statMaps;

    public CharacterStats()
    {
        _statMaps = new Dictionary<StatNames, float>()
        {
            [StatNames.AttackSpeed] = 1,
            [StatNames.Agility] = 1,
            [StatNames.AttackCooldown] = 2,
        };
    }

    public float GetStatValue(StatNames statName)
    {
        return _statMaps[statName];
    }
}
