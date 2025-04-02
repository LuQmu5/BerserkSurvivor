using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeDisplay : MonoBehaviour
{
    [SerializeField] private Image _icon;

    private UpgradeData _data;

    public void Init(UpgradeData data)
    {
        _data = data;

        _icon.sprite = data.Icon;
    }
}
