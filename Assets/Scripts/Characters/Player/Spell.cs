using System;
using UnityEngine;

public abstract class Spell
{
    public SpellData Data { get; private set; }

    public Spell(SpellData data)
    {
        Data = data;
    }

    public abstract void Use(ICaster caster);
}

public class FireballSpell : Spell
{
    public FireballSpell(SpellData data) : base(data)
    {
    }

    public override void Use(ICaster caster)
    {
        SpellProjectile fireball = ProjectileFactory.Instance.GetItem(ProjectileType.Fireball);
        fireball.Init(caster);
    }
}

public class FrostboltSpell : Spell
{
    public FrostboltSpell(SpellData data) : base(data)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + "frost magic solo towards projectile with pierce");
    }
}

public class ArcaneMissileSpell : Spell
{
    public ArcaneMissileSpell(SpellData data) : base(data)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " arcane magic auto-projectiles!");
    }
}

public class HealSpell : Spell
{
    public HealSpell(SpellData data) : base(data)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " self heal with life magic!");
    }
}

public class TrailOfLifeSpell : Spell
{
    public TrailOfLifeSpell(SpellData data) : base(data)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " creates the trail of life magic behind the character");
    }
}

public class EarthQuake : Spell
{
    public EarthQuake(SpellData data) : base(data)
    {
    }

    public override void Use(ICaster caster)
    {
        Debug.Log(ToString() + " creates a earth quake in a point of caster");
    }
}
