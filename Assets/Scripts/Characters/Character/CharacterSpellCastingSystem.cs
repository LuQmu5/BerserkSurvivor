﻿using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class CharacterSpellCastingSystem : ICaster
{
    private const int MaxComboLength = 3;
    private const float ComboTimeLimit = 1f;
    private const float ComboTimeUpdate = 0.5f;

    private SpellBook _spellBook;
    private IReadOnlyDictionary<KeyCode, MagicElements> _spellKeys;
    private MagicElements[] _currentCombo;
    private SpellCastView _spellBookView;
    private int _currentSigilIndex;

    private float _comboTimer;
    private bool _isComboReset;

    public Spell CurrentActiveSpell => _spellBook.CurrentActiveSpell;
    public Transform Transform { get; }
    public Transform CastPoint { get; }

    public CharacterSpellCastingSystem(SpellCastView spellBookView,
        Transform transform, Transform castPoint, SpellsViewFactory factory, ICharacterStats stats)
    {
        Transform = transform;
        CastPoint = castPoint;

        _spellKeys = new Dictionary<KeyCode, MagicElements>()
        {
            [KeyCode.Alpha1] = MagicElements.Arcane,
            [KeyCode.Alpha2] = MagicElements.Death,
            [KeyCode.Alpha3] = MagicElements.Earth,
            [KeyCode.Alpha4] = MagicElements.Fire,
            [KeyCode.Alpha5] = MagicElements.Frost,
            [KeyCode.Alpha6] = MagicElements.Inferno,
            [KeyCode.Alpha7] = MagicElements.Life,
            [KeyCode.Alpha8] = MagicElements.Wind,
        };

        _currentCombo = new MagicElements[MaxComboLength];
        _spellBook = new SpellBook(factory, stats);
        _spellBookView = spellBookView;

        _comboTimer = ComboTimeLimit;
        _isComboReset = false;
    }

    public void StartListening(ICoroutineRunner coroutineRunner)
    {
        coroutineRunner.StartCoroutine(Listening());
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
            UpdateComboTimer();

            yield return null;
        }
    }

    private void CheckComboKeys()
    {
        foreach (var key in _spellKeys.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                if (_comboTimer <= 0)
                {
                    _comboTimer = ComboTimeLimit; 
                }
                else
                {
                    _comboTimer = Mathf.Min(_comboTimer + ComboTimeUpdate, ComboTimeLimit); 
                }

                _isComboReset = false;
                FillComboArray(_spellKeys[key]);
                break;
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

        _spellBookView.UpdageSigilView(item, _currentSigilIndex);
        _currentCombo[_currentSigilIndex++] = item;

        if (_currentSigilIndex >= _currentCombo.Length)
            _currentSigilIndex = 0;
    }

    private void UpdateComboTimer()
    {
        if (_comboTimer > 0)
        {
            _comboTimer -= Time.deltaTime;
        }
        else if (!_isComboReset)
        {
            ResetCombo();
        }
    }

    private void ResetCombo()
    {
        _currentCombo = new MagicElements[MaxComboLength];
        _currentSigilIndex = 0;
        _spellBookView.DeactivateSigils();

        _isComboReset = true;
    }

    public void Cast()
    {
        CurrentActiveSpell.Use(this);
    }
}
