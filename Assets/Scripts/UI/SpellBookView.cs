using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookView : MonoBehaviour
{
    [Header("Current Spell View")]
    [SerializeField] private Image _currentSpellIcon;
    [SerializeField] private Image _filledCooldownSpellIcon;

    [Header("Sigils View")]
    [SerializeField] private SigilView[] _sigilsView;

    private int _currentActiveSigilViewIndex = 0;

    private void Start()
    {
        DeactivateSigils();
    }

    public void SetPattern(MagicElements element, int index)
    {
        _sigilsView[index].SetIconFor(element);

        _sigilsView[_currentActiveSigilViewIndex].DeactivateBorder();
        _currentActiveSigilViewIndex++;

        if (_currentActiveSigilViewIndex >= _sigilsView.Length)
            _currentActiveSigilViewIndex = 0;

        _sigilsView[_currentActiveSigilViewIndex].ActivateBorder();
        _sigilsView[_currentActiveSigilViewIndex].SetActiveAlpha();
    }

    public void SetNewSpellIcon(SpellData spell)
    {
        _currentSpellIcon.sprite = spell.Icon;
        _filledCooldownSpellIcon.sprite = spell.Icon;

        DeactivateSigils();
    }

    private void DeactivateSigils()
    {
        foreach (SigilView sigilView in _sigilsView)
            sigilView.SetIconFor(MagicElements.None);

        _sigilsView[0].SetActiveAlpha();
        _sigilsView[0].ActivateBorder();
    }

    public void UpdateCooldownFillAmount(float remainedCooldownPercent)
    {
        _filledCooldownSpellIcon.fillAmount = remainedCooldownPercent;
    }
}
