using UnityEngine;
using UnityEngine.UI;

public class SpellBookView : MonoBehaviour
{
    [SerializeField] private SigilView[] _sigilsView;
    [SerializeField] private Image _spellIcon;
    [SerializeField] private Image _spellCooldownIcon;

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
        _spellIcon.sprite = spell.Icon;
        _spellCooldownIcon.sprite = spell.Icon;
    }
}
