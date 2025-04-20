public class FireballSpell : Spell
{
    public FireballSpell(SpellData data, SpellsViewFactory projectileFactory, ICharacterStats casterStats)
        : base(data, projectileFactory, casterStats)
    {
    }

    public override void Use(ICaster caster)
    {
        SpellView fireball = SpellsViewVfactory.GetItem(SpellViewType.Fireball);
        fireball.Init(caster);
    }
}
