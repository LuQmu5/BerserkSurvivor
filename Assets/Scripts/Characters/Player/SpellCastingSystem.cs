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
    Wind = 8
}

public class SpellCastingSystem
{
    private const int MaxComboLength = 3;

    private IReadOnlyDictionary<KeyCode, MagicElements> _spellKeys;
    private MagicElements[] _currentCombo;
    private SpellBook _spellBook;
    private SpellBookView _spellBookView;
    private int _currentSpellIndex;

    public Spell CurrentActiveSpell => _spellBook.CurrentActiveSpell;

    public SpellCastingSystem(ICoroutineRunner coroutineRunner, SpellBookView spellBookView)
    {
        _spellKeys = new Dictionary<KeyCode, MagicElements>()
        {
            [KeyCode.Z] = MagicElements.Arcane,
            [KeyCode.X] = MagicElements.Death,
            [KeyCode.C] = MagicElements.Earth,
            [KeyCode.V] = MagicElements.Fire,
            [KeyCode.B] = MagicElements.Frost,
            [KeyCode.N] = MagicElements.Inferno,
            [KeyCode.M] = MagicElements.Life,
            [KeyCode.L] = MagicElements.Wind,
        };

        coroutineRunner.StartCoroutine(Listening());

        _currentCombo = new MagicElements[MaxComboLength];
        _spellBook = new SpellBook();
        _spellBookView = spellBookView;
    }

    public bool TryActiveSpell()
    {
        if (TryGetCurrentComboPattern(out MagicElementsPattern result))
        {
            if (_spellBook.TrySetActiveSpell(result))
            {
                _spellBookView.SetNewSpellIcon(_spellBook.CurrentActiveSpell.Data);
                return true;
            }
        }

        return false;
    }

    private bool TryGetCurrentComboPattern(out MagicElementsPattern result)
    {
        result = new MagicElementsPattern();

        if (_currentCombo[0] == default || _currentCombo[1] == default || _currentCombo[2] == default)
            return false;

        result = new MagicElementsPattern(_currentCombo[0], _currentCombo[1], _currentCombo[2]);

        return true;
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
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                if (Input.GetKeyDown(key))
                    if (_spellKeys.ContainsKey(key))
                        FillComboArray(_spellKeys[key]);
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

        _spellBookView.SetPattern(item, _currentSpellIndex);
        _currentCombo[_currentSpellIndex++] = item;

        if (_currentSpellIndex >= _currentCombo.Length)
            _currentSpellIndex = 0;


        Debug.Log($"" +
            $"1. {_currentCombo[0]}\n" +
            $"2. {_currentCombo[1]}\n" +
            $"3. {_currentCombo[2]}\n"
            );
    }

    public void Cast()
    {
        CurrentActiveSpell.Use();
    }
}
