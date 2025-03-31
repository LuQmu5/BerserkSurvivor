using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum MagicElements
{
    None = 0,
    Arcane = 1,
    Death = 2,
    Earth = 3,
    Fire = 4,
    Frost = 5,
    Inferno = 6,
    Life = 7,
    Light = 8,
}

public class SpellCastingSystem
{
    private const int MaxComboLength = 3;

    private IReadOnlyDictionary<KeyCode, MagicElements> _spellKeys;
    private MagicElements[] _currentCombo;

    public string CurrentComboPattern => GetCurrentComboPattern();

    public SpellCastingSystem(ICoroutineRunner coroutineRunner)
    {
        _spellKeys = new Dictionary<KeyCode, MagicElements>()
        {
            [KeyCode.Alpha1] = MagicElements.Arcane,
            [KeyCode.Alpha2] = MagicElements.Death,
            [KeyCode.Alpha3] = MagicElements.Earth,
            [KeyCode.Alpha4] = MagicElements.Fire,
            [KeyCode.Alpha5] = MagicElements.Frost,
            [KeyCode.Alpha6] = MagicElements.Inferno,
            [KeyCode.Alpha7] = MagicElements.Life,
            [KeyCode.Alpha8] = MagicElements.Light,

        };

        coroutineRunner.StartCoroutine(Listening());

        _currentCombo = new MagicElements[MaxComboLength];
    }

    private string GetCurrentComboPattern()
    {
        string result = "";

        foreach (var spell in _currentCombo)
        {
            result += spell;
        }

        return result;
    }

    private IEnumerator Listening()
    {
        while (true)
        {
            CheckComboKeys();

            yield return null;
        }
    }

    private void CheckComboKeys()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key) && _spellKeys.ContainsKey(key))
                {
                    FillComboArray(_spellKeys[key]);
                }
            }
        }
    }

    private void FillComboArray(MagicElements item)
    {
        for (int i = 0; i < _currentCombo.Length; i++)
        {
            if (_currentCombo[i] == default)
            {
                _currentCombo[i] = item;
                break;
            }
        }

        _currentCombo[0] = item;
    }
}

public class SpellBook
{
    private List<Spell> _spells;

    public SpellBook()
    {
        _spells = new List<Spell>();


    }
}

public class Spell
{
    public Spell()
    {
        
    }
}

[CreateAssetMenu(menuName = "StaticData/Spells/New Spell Data", fileName = "New Spell Data", order = 54)]
public class SpellData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}

public struct MagicElementsPattern
{
    public MagicElements First { get; private set; }
    public MagicElements Second { get; private set; }
    public MagicElements Third { get; private set; }

    public MagicElementsPattern(MagicElements first, MagicElements second, MagicElements third)
    {
        first = First;
        second = Second;
        third = Third;
    }
}