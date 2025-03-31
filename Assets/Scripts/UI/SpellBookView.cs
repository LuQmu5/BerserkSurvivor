using UnityEngine;
using UnityEngine.UI;

public class SpellBookView : MonoBehaviour
{
    [SerializeField] private SigilView[] _sigilsView;
    [SerializeField] private Image _spellIcon;
    [SerializeField] private Image _spellCooldownIcon;

    public void Init(SpellData startSpell)
    {
        SetPattern(startSpell.Pattern.First, 0);
        SetPattern(startSpell.Pattern.Second, 1);
        SetPattern(startSpell.Pattern.Third, 2);

        SetSpellIcon(startSpell);
    }

    public void SetPattern(MagicElements element, int index)
    {
        _sigilsView[index].SetIconFor(element);
    }

    public void SetSpellIcon(SpellData spell)
    {
        _spellIcon.sprite = spell.Icon;
        _spellCooldownIcon.sprite = spell.Icon;
    }
}
