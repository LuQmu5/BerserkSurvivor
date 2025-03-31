using System;
using UnityEngine;

public class Spell
{
    public SpellData Data { get; private set; }

    public Spell(SpellData data)
    {
        Data = data;
    }

    public void Use()
    {
        Debug.Log(Data.Name + " used!");
    }
}
