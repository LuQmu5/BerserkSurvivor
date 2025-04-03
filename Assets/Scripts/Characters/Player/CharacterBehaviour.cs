using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

// надо делить еще на более мелкие системы и классы
public class CharacterBehaviour : MonoBehaviour, IHealth, ICoroutineRunner, IItemPicker, ILevelable
{
    private const string HorizintalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    [SerializeField] private Transform _backpackPoint;

    private PlayerInput _input;
    private CharacterStats _stats;

    private CharacterCombatSystem _combatSystem;
    private CharacterMover _mover;
    private CharacterView _view;

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public float MaxLevel { get; private set; } = 10;
    public float CurrentLevel { get; private set; } = 1;
    public float CurrentExpirience { get; private set; } = 0;
    public float ExpirienceForNextLevel { get => CurrentLevel * 100; }

    public void Init(PlayerInput input, CharacterStats stats, SpellBookView spellBookView)
    {
        _input = input;
        _input.Enable();

        _stats = stats;

        _view = new CharacterView(GetComponent<Animator>());
        _combatSystem = new CharacterCombatSystem(this, stats, spellBookView, _view);
        _mover = new CharacterMover(GetComponent<CharacterController>(), this, stats);

        MaxHealth = 10;
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        Vector3 inputVector = Camera.main.transform.TransformDirection(GetInputAxis());

        // проверки на разные типы атак (с шифтом и контрол) и вызов разных методов, которые вызывают разные методы юзания спелла 
        if (_input.Combat.Attack.inProgress && _combatSystem.AttackOnCooldown == false)
            TryAttack();
        else
            Move(inputVector);

        if (_input.Combat.ActivateSpell.triggered)
            _combatSystem.TryActivateSpell();

        Rotate(inputVector, toMouse: _input.Combat.Attack.inProgress && _combatSystem.AttackOnCooldown == false);
        _stats.TryGetCurrentValueOfStat(StatNames.AttackSpeed, out float attackSpeed);
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
        _combatSystem.TryStartAttackPerform();
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

        // event
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
}

public interface ICoroutineRunner
{
    public Coroutine StartCoroutine(IEnumerator routineName);
    public void StopCoroutine(Coroutine coroutine);
}

