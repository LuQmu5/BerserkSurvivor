using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

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

    public SpellBook(ProjectileFactory factory)
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
            new FireballSpell(fireballData, factory),
            new FrostboltSpell(frostboltData, factory),
            new ArcaneMissileSpell(arcaneMissileData, factory),
            new HealSpell(healData, factory),
            new TrailOfLifeSpell(trailOfLifeData, factory),
            new EarthQuake(earthQuakeData, factory),
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
