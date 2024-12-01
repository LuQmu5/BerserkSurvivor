using System;
using UnityEngine;

public class UpgradesGetTest : MonoBehaviour
{
    public event Action UpgradeAvailable;

    private void OnMouseDown()
    {
        print("upgrade");
        UpgradeAvailable?.Invoke();
    }
}
