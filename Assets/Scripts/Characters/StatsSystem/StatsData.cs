using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Stat Data", menuName = "StaticData/Characters/New Character Stat Data", order = 54)]
public class StatsData : ScriptableObject
{
    [field: SerializeField] public Stat[] Stats = GenerateStatsBaseMap();

    private static Stat[] GenerateStatsBaseMap()
    {
        Array allStatsNames = Enum.GetValues(typeof(StatNames));

        Stat[] stats = new Stat[allStatsNames.Length];

        for (int i = 0; i <stats.Length; i++)
        {
            stats[i] = new Stat((StatNames)allStatsNames.GetValue(i), 0);
        }

        return stats;
    }
}

// attack speed = 10 too fast, 1 simple, x100 multiplier for UI
