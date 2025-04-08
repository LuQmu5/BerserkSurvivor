using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSpellCastingSystem : ICaster
{
    private SpellBook _spellBook;
    public Spell CurrentActiveSpell => _spellBook.CurrentActiveSpell;
    public Transform Transform { get; }
    public Transform CastPoint { get; }

    private const int MaxComboLength = 3;
    private const float ComboTimeLimit = 1f; // 1 секунда на сбор комбинации
    private const float ComboTimeUpdate = 0.33f; // 0.33 сек для обновления таймера

    private IReadOnlyDictionary<KeyCode, MagicElements> _spellKeys;
    private MagicElements[] _currentCombo;
    private SpellBookView _spellBookView;
    private int _currentSpellIndex;

    private float _comboTimer; // Таймер для отслеживания времени на сбор комбинации


    public CharacterSpellCastingSystem(ICoroutineRunner coroutineRunner, SpellBookView spellBookView,
        Transform transform, Transform castPoint, SpellsViewFactory factory, CharacterStats stats)
    {
        Transform = transform;
        CastPoint = castPoint;

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

        _currentCombo = new MagicElements[MaxComboLength];
        _spellBook = new SpellBook(factory, stats);
        _spellBookView = spellBookView;

        _comboTimer = ComboTimeLimit; // Инициализация таймера
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
        if (Input.anyKeyDown)
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                if (Input.GetKeyDown(key))
                    if (_spellKeys.ContainsKey(key))
                        FillComboArray(_spellKeys[key]);
    }

    private void FillComboArray(MagicElements item)
    {
        // Сбрасываем таймер, так как мы нажали клавишу
        _comboTimer = ComboTimeLimit;

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
    }

    private void UpdateComboTimer()
    {
        // Если таймер больше 0, уменьшать его
        if (_comboTimer > 0)
        {
            _comboTimer -= Time.deltaTime;
        }
        else
        {
            // Если таймер истек, сбрасываем комбинацию
            ResetCombo();
        }
    }

    private void ResetCombo()
    {
        _currentCombo = new MagicElements[MaxComboLength];
        _currentSpellIndex = 0;
        _spellBookView.DeactivateSigils(); // Сбрасываем UI
    }

    public void Cast()
    {
        CurrentActiveSpell.Use(this);
    }
}
