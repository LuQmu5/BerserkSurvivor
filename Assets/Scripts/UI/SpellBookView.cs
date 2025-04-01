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
        _currentActiveSigilViewIndex = 0;
        _sigilsView[_currentActiveSigilViewIndex].ActivateBorder();
    }

    public void SetPattern(MagicElements element, int index)
    {
        _sigilsView[index].SetIconFor(element);

        _sigilsView[_currentActiveSigilViewIndex].DeactivateBorder();
        _currentActiveSigilViewIndex++;

        if (_currentActiveSigilViewIndex >= _sigilsView.Length)
            _currentActiveSigilViewIndex = 0;

        _sigilsView[_currentActiveSigilViewIndex].ActivateBorder();
    }

    public void SetSpellIcon(SpellData spell)
    {
        _currentSpellIcon.sprite = spell.Icon;
        _filledCooldownSpellIcon.sprite = spell.Icon;
    }

    public void UpdateCooldownFillAmount(float remainedCooldownPercent)
    {
        _filledCooldownSpellIcon.fillAmount = remainedCooldownPercent;
    }
}
