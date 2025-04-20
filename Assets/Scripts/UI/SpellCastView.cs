using UnityEngine;
using UnityEngine.UI;

public class SpellCastView : MonoBehaviour
{
    [Header("Current Spell View")]
    [SerializeField] private Image _currentSpellIcon;
    [SerializeField] private Image _filledCooldownSpellIcon;

    [Header("Sigils View")]
    [SerializeField] private SigilView[] _sigilsView;

    private void Start()
    {
        DeactivateSigils();
    }

    public void UpdageSigilView(MagicElements element, int index)
    {
        _sigilsView[index].UpdateIconWith(element);

        if (index != _sigilsView.Length - 1)
            _sigilsView[index + 1].SetActive(true);
    }

    public void SetNewSpellIcon(SpellData spell)
    {
        _currentSpellIcon.sprite = spell.Icon;
        _filledCooldownSpellIcon.sprite = spell.Icon;

        DeactivateSigils();
    }

    public void DeactivateSigils()
    {
        foreach (SigilView sigilView in _sigilsView)
        {
            sigilView.UpdateIconWith(MagicElements.None);
            sigilView.SetActive(false);
        }

        _sigilsView[0].SetActive(true);
    }

    public void UpdateCooldownFillAmount(float remainedCooldownPercent)
    {
        _filledCooldownSpellIcon.fillAmount = remainedCooldownPercent;
    }
}
