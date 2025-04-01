using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SpellNames
{
    Fireball,
    Frostbolt,
    ArcaneMissile,
    Heal,
    TrailOfLife
}

public class SpellBook
{
    private const string SpellsPath = "StaticData/Spells";

    private List<Spell> _spells;

    public Spell CurrentActiveSpell { get; private set; } = null;

    public SpellBook()
    {
        SpellData[] spellsData = Resources.LoadAll<SpellData>(SpellsPath);

        SpellData fireball = spellsData.First(i => i.Name == SpellNames.Fireball);
        SpellData frostbolt = spellsData.First(i => i.Name == SpellNames.Frostbolt);
        SpellData arcaneMissile = spellsData.First(i => i.Name == SpellNames.ArcaneMissile);
        SpellData heal = spellsData.First(i => i.Name == SpellNames.Heal);
        SpellData trailOfLife = spellsData.First(i => i.Name == SpellNames.TrailOfLife);

        _spells = new List<Spell>()
        {
            new FireballSpell(fireball),
            new FrostboltSpell(frostbolt),
            new ArcaneMissileSpell(arcaneMissile),
            new HealSpell(heal),
            new TrailOfLifeSpell(trailOfLife)
        };

        CurrentActiveSpell = null;
    }

    public bool TrySetActiveSpell(MagicElementsPattern pattern)
    {
        Spell spell = _spells.Find(i => i.Data.Pattern == pattern);

        if (spell == null)
        {
            return false;
        }

        CurrentActiveSpell = spell;
        Debug.Log(spell.Data.Name + " is activated");

        return true;
    }
}
