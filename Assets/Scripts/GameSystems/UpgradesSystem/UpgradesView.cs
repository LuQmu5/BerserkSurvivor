using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesView : MonoBehaviour
{
    [SerializeField] private UpgradeDisplay _upgradeDisplayPrefab;
    [SerializeField] private HorizontalLayoutGroup _container;
    [SerializeField] private Button _selectButton;

    private void OnEnable()
    {
        _selectButton.onClick.AddListener(OnSelectButtonClicked);
    }

    private void OnDisable()
    {
        _selectButton.onClick.RemoveListener(OnSelectButtonClicked);
    }

    public void Show(IEnumerable upgrades)
    {
        gameObject.SetActive(true);

        foreach (UpgradeDisplay upgradeDisplay in _container.GetComponentsInChildren<UpgradeDisplay>())
        {
            Destroy(upgradeDisplay.gameObject);
        }

        foreach (UpgradeData data in upgrades)
        {
            UpgradeDisplay newUpgrade = Instantiate(_upgradeDisplayPrefab, _container.transform);
            newUpgrade.Init(data);
        }
    }

    private void OnSelectButtonClicked()
    {
        gameObject.SetActive(false);
    }
}
