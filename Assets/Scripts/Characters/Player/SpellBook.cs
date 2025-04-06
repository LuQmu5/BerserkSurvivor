using System.Collections.Generic;
using UnityEngine;

public class SpellBook
{
    private const string SpellsPath = "StaticData/Spells";

    private List<Spell> _spells;
    public Spell CurrentActiveSpell { get; private set; } = null;

    public SpellBook(SpellsViewFactory factory, CharacterStats stats)
    {
        SpellFactory spellFactory = new SpellFactory(factory, stats);
        SpellData[] spellsData = Resources.LoadAll<SpellData>(SpellsPath); // #config

        _spells = new List<Spell>();

        foreach (var data in spellsData)
        {
            try
            {
                Spell spell = spellFactory.CreateSpell(data);
                _spells.Add(spell);
            }
            catch (System.NotImplementedException e)
            {
                Debug.LogWarning($"SpellBook: {e.Message}");
            }
        }

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
        return true;
    }
}
