using System.Collections;
using UnityEngine;
using Zenject;

public class CharacterBehaviour : MonoBehaviour, IHealth, ICoroutineRunner
{
    private const string HorizintalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";


    private PlayerInput _input;
    private CharacterStats _stats;
    private Animator _animator;

    private CharacterMagicCombatSystem _combatSystem;
    private CharacterMover _mover;
    private CharacterView _view;

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    public void Init(PlayerInput input, CharacterStats stats, SpellBookView spellBookView)
    {
        print("construct");

        _input = input;
        _input.Enable();

        _stats = stats;

        _view = new CharacterView(GetComponent<Animator>());
        _combatSystem = new CharacterMagicCombatSystem(this, stats, spellBookView);
        _mover = new CharacterMover(GetComponent<CharacterController>(), 10, 10, this);

        MaxHealth = 10;
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        Vector3 inputVector = Camera.main.transform.TransformDirection(GetInputAxis());

        Rotate(inputVector);
        Move(inputVector);

        // проверки на разные типы атак (с шифтом и контрол) и вызов разных методов, которые вызывают разные методы юзания спелла 
        if (_input.Combat.Attack.triggered)
        {
            TryAttack();
            _mover.FreezeMovementFor(GameConfig.StartAttackFreezeTreshold);
        }

        if (_input.Combat.ActivateSpell.triggered)
            _combatSystem.TryActivateSpell();

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
        _view.CallEndOfAttackAnimation(isBreaked: false);
    }

    private void Rotate(Vector3 inputVector)
    {
        if (_view.AttackInProgress)
        {
            _mover.RotateToMouse();
        }
        else
        {
            _mover.RotateToPoint(inputVector);
        }
    }

    private bool TryAttack()
    {
        if (_view.AttackInProgress)
            return false;

        if (_combatSystem.TryStartAttack())
        {
            _view.PlayAttackAnimation();
            _view.CurrentAnimationPerformed += PerformAttack;

            return true;
        }

        return false;
    }

    private void PerformAttack(ICharacterView viewCaller)
    {
        _combatSystem.PerformAttack();
        viewCaller.CurrentAnimationPerformed -= PerformAttack;
    }

    private void Move(Vector3 inputVector)
    {
        if (_mover.TryMove(inputVector, _view.AttackInProgress))
        {
            bool predicateToRun = inputVector.sqrMagnitude > 0; // && _mover.MoveIsFreezed == false;
            _view.SetRunningState(predicateToRun);
        }
    }

    private Vector3 GetInputAxis() => new Vector2(SimpleInput.GetAxis(HorizintalAxis), SimpleInput.GetAxis(VerticalAxis));
}

public interface ICoroutineRunner
{
    public Coroutine StartCoroutine(IEnumerator routineName);
    public void StopCoroutine(Coroutine coroutine);
}
