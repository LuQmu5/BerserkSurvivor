using UnityEngine;

public abstract class Spell
{
    public SpellData Data { get; private set; }

    protected ICharacterStats CasterStats { get; }

    protected SpellsViewFactory SpellsViewVfactory;

    public Spell(SpellData data, SpellsViewFactory projectileFactory, ICharacterStats casterStats)
    {
        Data = data;
        SpellsViewVfactory = projectileFactory;
        CasterStats = casterStats;
    }

    public abstract void Use(ICaster caster);
}

public class HasteBuff : Spell
{
    public HasteBuff(SpellData data, SpellsViewFactory projectileFactory, ICharacterStats casterStats)
        : base(data, projectileFactory, casterStats)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + ": speed buff");
    }
}

public class FrostboltSpell : Spell
{
    public FrostboltSpell(SpellData data, SpellsViewFactory projectileFactory, ICharacterStats casterStats)
        : base(data, projectileFactory, casterStats)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " frost magic solo towards projectile with pierce");
    }
}

public class ArcaneMissileSpell : Spell
{
    public ArcaneMissileSpell(SpellData data, SpellsViewFactory projectileFactory, ICharacterStats casterStats)
        : base(data, projectileFactory, casterStats)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " arcane magic auto-projectiles!");
    }
}

public class HealSpell : Spell
{
    public HealSpell(SpellData data, SpellsViewFactory projectileFactory, ICharacterStats casterStats)
        : base(data, projectileFactory, casterStats)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " self heal with life magic!");
    }
}

public class TrailOfLifeSpell : Spell
{
    public TrailOfLifeSpell(SpellData data, SpellsViewFactory projectileFactory, ICharacterStats casterStats)
        : base(data, projectileFactory, casterStats)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " creates the trail of life magic behind the character");
    }
}

public class EarthQuake : Spell
{
    public EarthQuake(SpellData data, SpellsViewFactory projectileFactory, ICharacterStats casterStats)
        : base(data, projectileFactory, casterStats)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " creates an earthquake at a point of caster");
    }
}
