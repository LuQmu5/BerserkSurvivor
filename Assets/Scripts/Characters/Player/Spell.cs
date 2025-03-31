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
        Debug.Log(ToString() + "FIRE!");
    }
}

public class FrostboltSpell : Spell
{
    public FrostboltSpell(SpellData data) : base(data)
    {
    }

    public override void Use()
    {
        Debug.Log(ToString() + "ICE!");
    }
}
