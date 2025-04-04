using System;
using UnityEngine;

public abstract class Spell
{
    public SpellData Data { get; private set; }

    protected ProjectileFactory Factory;

    public Spell(SpellData data, ProjectileFactory projectileFactory)
    {
        Data = data;
        Factory = projectileFactory;
    }

    public abstract void Use(ICaster caster);
}

public class FireballSpell : Spell
{
    public FireballSpell(SpellData data, ProjectileFactory projectileFactory) : base(data, projectileFactory)
    {
    }

    public override void Use(ICaster caster)
    {
        SpellProjectile fireball = Factory.GetItem(ProjectileType.Fireball);
        fireball.Init(caster);
    }
}

public class FrostboltSpell : Spell
{
    public FrostboltSpell(SpellData data, ProjectileFactory projectileFactory) : base(data, projectileFactory)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + "frost magic solo towards projectile with pierce");
    }
}

public class ArcaneMissileSpell : Spell
{
    public ArcaneMissileSpell(SpellData data, ProjectileFactory projectileFactory) : base(data, projectileFactory)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " arcane magic auto-projectiles!");
    }
}

public class HealSpell : Spell
{
    public HealSpell(SpellData data, ProjectileFactory projectileFactory) : base(data, projectileFactory)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " self heal with life magic!");
    }
}

public class TrailOfLifeSpell : Spell
{
    public TrailOfLifeSpell(SpellData data, ProjectileFactory projectileFactory) : base(data, projectileFactory)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " creates the trail of life magic behind the character");
    }
}

public class EarthQuake : Spell
{
    public EarthQuake(SpellData data, ProjectileFactory projectileFactory) : base(data, projectileFactory)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " creates a earth quake in a point of caster");
    }
}
