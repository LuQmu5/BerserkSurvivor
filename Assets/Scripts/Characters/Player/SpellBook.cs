using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SpellNames
{
    Fireball,
    Frostbolt,
    ArcaneMissile,
    Heal,
    TrailOfLife,
    EarthQuake
}

public class SpellBook
{
    private const string SpellsPath = "StaticData/Spells";

    private List<Spell> _spells;

    public Spell CurrentActiveSpell { get; private set; } = null;

    public SpellBook()
    {
        SpellData[] spellsData = Resources.LoadAll<SpellData>(SpellsPath);

        SpellData fireballData = spellsData.First(i => i.Name == SpellNames.Fireball);
        SpellData frostboltData = spellsData.First(i => i.Name == SpellNames.Frostbolt);
        SpellData arcaneMissileData = spellsData.First(i => i.Name == SpellNames.ArcaneMissile);
        SpellData healData = spellsData.First(i => i.Name == SpellNames.Heal);
        SpellData trailOfLifeData = spellsData.First(i => i.Name == SpellNames.TrailOfLife);
        SpellData earthQuakeData = spellsData.First(i => i.Name == SpellNames.EarthQuake);

        _spells = new List<Spell>()
        {
            new FireballSpell(fireballData),
            new FrostboltSpell(frostboltData),
            new ArcaneMissileSpell(arcaneMissileData),
            new HealSpell(healData),
            new TrailOfLifeSpell(trailOfLifeData),
            new EarthQuake(earthQuakeData),
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
        // Debug.Log(spell.Data.Name + " is activated");

        return true;
    }
}
