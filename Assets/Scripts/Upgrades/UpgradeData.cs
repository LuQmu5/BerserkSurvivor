using UnityEngine;

[CreateAssetMenu(menuName = "StaticData/Upgrades/New Upgrade", fileName = "New Upgrade Data", order = 54)]
public class UpgradeData : ScriptableObject
{
    [field: SerializeField] public string Header { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}
