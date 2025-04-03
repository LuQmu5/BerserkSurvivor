using DG.Tweening;
using System.Collections;
using UnityEngine;
using Zenject;

public class CharacterBehaviour : MonoBehaviour, IHealth, ICoroutineRunner, IItemPicker
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

    public void Init(PlayerInput input, CharacterStats stats, SpellBookView spellBookView)
    {
        _input = input;
        _input.Enable();

        _stats = stats;

        _view = new CharacterView(GetComponent<Animator>());
        _combatSystem = new CharacterCombatSystem(this, stats, spellBookView, _view);
        _mover = new CharacterMover(GetComponent<CharacterController>(), 10, 10, this);

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
        float baseDuration = 2f;
        float finalSizeCoeff = 0.2f;
        float minDistance = 0.2f;

        float moveDuration = baseDuration;
        Vector3 originalScale = item.transform.localScale;

        while (Vector3.Distance(item.transform.position, _backpackPoint.transform.position) > minDistance)
        {
            moveDuration -= (Time.deltaTime * Time.deltaTime) / baseDuration;

            item.transform.DOMove(_backpackPoint.position, moveDuration)
                        .SetEase(Ease.InExpo)
                        .SetLoops(-1, LoopType.Restart)
                        .OnUpdate(() => item.transform.DOMove(_backpackPoint.position, moveDuration));

            item.transform.DOScale(originalScale * finalSizeCoeff, moveDuration)
                          .SetEase(Ease.InExpo);

            yield return null;
        }

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
}

public interface ICoroutineRunner
{
    public Coroutine StartCoroutine(IEnumerator routineName);
    public void StopCoroutine(Coroutine coroutine);
}
