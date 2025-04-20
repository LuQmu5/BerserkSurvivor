using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// надо делить еще на более мелкие системы и классы
public class CharacterBehaviour : MonoBehaviour, IHealth, ICoroutineRunner, IItemPicker, ILevelable, ICharacterStats
{
    private const string HorizintalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    [SerializeField] private Transform _backpackPoint;
    [SerializeField] private Transform _castPoint;

    private PlayerInput _input;
    private CharacterCombatSystem _combatSystem;
    private CharacterMover _mover;
    private CharacterView _view;

    private readonly Dictionary<StatNames, Stat> _baseStats = new();
    private readonly Dictionary<StatNames, List<StatModifier>> _modifiers = new();

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float MaxLevel { get; private set; } = 10;
    public float CurrentLevel { get; private set; } = 1;
    public float CurrentExpirience { get; private set; } = 0;
    public float ExpirienceForNextLevel { get => CurrentLevel * 100; }

    public Transform Transform => transform;

    public void Init(PlayerInput input, SpellCastView spellBookView, StatsData data, SpellsViewFactory factory)
    {
        _input = input;
        _input.Enable();

        foreach (Stat stat in data.Stats)
        {
            _baseStats.Add(stat.Name, stat);
        }

        _view = new CharacterView(GetComponent<Animator>());
        _combatSystem = new CharacterCombatSystem(this, this, spellBookView, _view, transform, _castPoint, factory);
        _mover = new CharacterMover(GetComponent<CharacterController>(), this);

        TryGetCurrentValueOfStat(StatNames.MaxHealth, out float maxHealth);
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        Vector3 inputVector = Camera.main.transform.TransformDirection(GetInputAxis());
        Rotate(inputVector, toMouse: _input.Combat.Attack.inProgress);

        // проверки на разные типы атак (с шифтом и контрол) и вызов разных методов, которые вызывают разные методы юзания спелла 
        if (_input.Combat.Attack.inProgress && _combatSystem.AttackOnCooldown == false)
        {
            _mover.InstaRotateToMouse();
            TryAttack();
        }
        else
        {
            Move(inputVector);
        }

        if (_input.Combat.ActivateSpell.triggered)
            _combatSystem.TryActivateSpell();

        TryGetCurrentValueOfStat(StatNames.AttackSpeed, out float attackSpeed);
        _view.SetAttackSpeedMultiplier(attackSpeed);
    }

    public void ApplyDamage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        if (CurrentHealth == 0)
            gameObject.SetActive(false);
    }

    /// <summary>
    /// calling in animations events, maybe make with routines and timers
    /// </summary>
    public void AttackAnimationPerformed()
    {
        _view.CallEndOfAttackAnimation(isCanceled: false);
    }

    private void Rotate(Vector3 inputVector, bool toMouse)
    {
        _mover.Rotate(inputVector, toMouse);
    }

    private void TryAttack()
    {
        if (_combatSystem.TryStartAttackPerform())
            _mover.InstaRotateToMouse();

        _view.SetRunningState(false);
    }

    private void Move(Vector3 inputVector)
    {
        bool isRunning = _mover.TryMove(inputVector, _view.AttackAnimationInProgress);
        _view.SetRunningState(isRunning);
    }

    private Vector3 GetInputAxis() => new Vector2(SimpleInput.GetAxis(HorizintalAxis), SimpleInput.GetAxis(VerticalAxis));

    public void PickUp(Item item)
    {
        StartCoroutine(PickingUp(item));
    }

    private IEnumerator PickingUp(Item item)
    {
        float baseDuration = 1f;       
        float finalSizeCoeff = 0.3f;      
        float elapsedTime = 0f;        

        Vector3 originalScale = item.transform.localScale;
        Vector3 startPos = item.transform.position;

        while (elapsedTime < baseDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / baseDuration;
            float earlyBoost = Mathf.Clamp01(t * 1.5f);
            float speedMultiplier = Mathf.Lerp(earlyBoost, Mathf.Pow(t, 2), t);

            item.transform.position = Vector3.Lerp(startPos, _backpackPoint.position, speedMultiplier);
            item.transform.localScale = Vector3.Lerp(originalScale, originalScale * finalSizeCoeff, t);

            yield return null;
        }

        item.transform.position = _backpackPoint.position;
        item.transform.localScale = originalScale * finalSizeCoeff;

        item.OnPickedUp(this);
    }

    public void Restore(float value)
    {
        CurrentHealth += value;

        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;

        Debug.Log("Healed");
    }

    public void AddExpirience(float amount)
    {
        if (CurrentLevel == MaxLevel)
            return;

        CurrentExpirience += amount;

        while (CurrentExpirience > ExpirienceForNextLevel)
        {
            CurrentLevel++;
            CurrentExpirience = CurrentExpirience - ExpirienceForNextLevel;
            OnLevelUp();
        }
    }

    public void OnLevelUp()
    {
        Debug.Log("Current level: " + CurrentLevel);
    }

    public float GetCurrentStatValue(StatNames statName)
    {
        if (!_baseStats.TryGetValue(statName, out Stat baseStat))
            return 0;

        float baseValue = baseStat.Value;
        float multiplier = 1f;

        if (_modifiers.TryGetValue(statName, out var modList))
        {
            CleanUpExpiredModifiers(modList);

            foreach (var mod in modList)
            {
                multiplier *= mod.Multiplier;
            }
        }

        return baseValue * multiplier;
    }

    public void AddTemporaryMultiplier(StatNames statName, float multiplier, float duration)
    {
        if (!_modifiers.ContainsKey(statName))
            _modifiers[statName] = new List<StatModifier>();

        var modifier = new StatModifier(multiplier, Time.time + duration);
        _modifiers[statName].Add(modifier);
    }

    public void CleanUpExpiredModifiers(List<StatModifier> modifiers)
    {
        modifiers.RemoveAll(mod => Time.time > mod.EndTime);
    }

    public bool TryGetCurrentValueOfStat(StatNames statName, out float result)
    {
        result = GetCurrentStatValue(statName);
        return result > 0;
    }
}
