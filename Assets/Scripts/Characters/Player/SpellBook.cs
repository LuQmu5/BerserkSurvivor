using System.Collections.Generic;
using UnityEngine;

public class SpellBook
{
    private const string SpellsPath = "StaticData/Spells";

    private List<Spell> _spells;

    public Spell CurrentActiveSpell { get; private set; }

    public SpellBook()
    {
        _spells = new List<Spell>();

        SpellData[] spellsData = Resources.LoadAll<SpellData>(SpellsPath);

        foreach (SpellData data in spellsData)
        {
            Spell spell = new Spell(data);
            _spells.Add(spell);
        }
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
