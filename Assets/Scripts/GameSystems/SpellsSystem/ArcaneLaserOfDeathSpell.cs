using UnityEngine;

public class ArcaneLaserOfDeathSpell : Spell
{
    public ArcaneLaserOfDeathSpell(SpellData data, SpellsViewFactory projectileFactory, ICharacterStats casterStats)
        : base(data, projectileFactory, casterStats)
    {
    }

    public override void Use(ICaster caster)
    {
        SpellView arcaneLaserOfDeath = SpellsViewVfactory.GetItem(SpellViewType.ArcaneLaserOfDeath);
        arcaneLaserOfDeath.Init(caster);
    }
}
