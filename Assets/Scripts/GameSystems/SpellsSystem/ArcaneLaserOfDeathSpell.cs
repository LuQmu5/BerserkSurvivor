using UnityEngine;

public class ArcaneLaserOfDeathSpell : Spell
{
    public ArcaneLaserOfDeathSpell(SpellData data, SpellsViewFactory projectileFactory, CharacterStats casterStats)
        : base(data, projectileFactory, casterStats)
    {
    }

    public override void Use(ICaster caster)
    {
        SpellView fireball = SpellsViewVfactory.GetItem(SpellViewType.ArcaneLaserOfDeath);
        fireball.Init(caster);
    }
}
