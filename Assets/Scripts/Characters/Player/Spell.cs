using System;
using UnityEngine;

public abstract class Spell
{
    public SpellData Data { get; private set; }

    public Spell(SpellData data)
    {
        Data = data;
    }

    public abstract void Use();
}

public class FireballSpell : Spell
{
    public FireballSpell(SpellData data) : base(data)
    {
    }

    public override void Use()
    {
        Debug.Log(ToString() + " fire magic towards projectile with explosion damage in area in point of collision");
    }
}

public class FrostboltSpell : Spell
{
    public FrostboltSpell(SpellData data) : base(data)
    {
    }

    public override void Use()
    {
        Debug.Log(ToString() + "frost magic solo towards projectile with pierce");
    }
}

public class ArcaneMissileSpell : Spell
{
    public ArcaneMissileSpell(SpellData data) : base(data)
    {
    }

    public override void Use()
    {
        Debug.Log(ToString() + " arcane magic auto-projectiles!");
    }
}

public class HealSpell : Spell
{
    public HealSpell(SpellData data) : base(data)
    {
    }

    public override void Use()
    {
        Debug.Log(ToString() + " self heal with life magic!");
    }
}

public class TrailOfLifeSpell : Spell
{
    public TrailOfLifeSpell(SpellData data) : base(data)
    {
    }

    public override void Use()
    {
        Debug.Log(ToString() + " creates the trail of life magic behind the character");
    }
}

public class EarthQuake : Spell
{
    public EarthQuake(SpellData data) : base(data)
    {
    }

    public override void Use()
    {
        Debug.Log(ToString() + " creates a earth quake in a point of caster");
    }
}