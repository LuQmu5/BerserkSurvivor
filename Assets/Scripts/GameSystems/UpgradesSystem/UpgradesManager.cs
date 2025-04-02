using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradesManager
{
    private const string UpgradesPath = "StaticData/Upgrades";

    private UpgradeData[] _upgrades;

    public UpgradesManager()
    {
        _upgrades = Resources.LoadAll<UpgradeData>(UpgradesPath);
    }

    public IEnumerable GetUpgradesData()
    {
        List<UpgradeData> upgradesTemp = new List<UpgradeData>();
        List<UpgradeData> upgradesList = _upgrades.ToList();

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, upgradesList.Count);
            upgradesTemp.Add(upgradesList[randomIndex]);
            upgradesList.RemoveAt(randomIndex);
        }

        return upgradesTemp;
    }
}
