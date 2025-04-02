using System;
using System.Collections;
using Zenject;
using UnityEngine;

public class UpgradesMediator : IDisposable
{
    private readonly UpgradesGetTest _upgradesGetTest;
    private readonly UpgradesView _upgradesView;
    private readonly UpgradesManager _upgradesManager;

    [Inject]
    public UpgradesMediator(UpgradesGetTest upgradesGetTest, UpgradesView upgradesView, UpgradesManager upgradesManager)
    {
        _upgradesGetTest = upgradesGetTest;
        _upgradesView = upgradesView;
        _upgradesManager = upgradesManager;

        _upgradesGetTest.UpgradeAvailable += OnUpgradeAvailable;
    }

    public void Dispose()
    {
        _upgradesGetTest.UpgradeAvailable -= OnUpgradeAvailable;
    }

    private void OnUpgradeAvailable()
    {
        IEnumerable upgrades = _upgradesManager.GetUpgradesData();
        _upgradesView.Show(upgrades);
    }
}