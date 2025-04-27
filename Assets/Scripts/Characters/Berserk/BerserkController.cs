using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class BerserkController : MonoBehaviour, IHealth
{
    [SerializeField] private BerserkAnimator _animator;
    [SerializeField] private BerserkCombat _combat;
    [SerializeField] private BerserkMover _mover;

    private PlayerInput _input;

    public float MaxHealth { get; private set; } = 10;
    public float CurrentHealth { get; private set; }

    public Transform Transform => transform;

    [Inject]
    public void Construct(PlayerInput input)
    {
        _input = input;
        _input.Enable();

        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        Vector3 inputVector = Camera.main.transform.TransformDirection(GetInputAxis());

        if (_combat.ProcessingDash)
            return;

        Move(inputVector); 
        
        if (_animator.AttackInProgress)
        {
            _mover.RotateToMouse();
        }
        else
        {
            _mover.RotateToPoint(inputVector);
        }

        if (_input.Combat.Attack.triggered)
        {
            Attack();
        }

        if (_input.Combat.Dash.triggered)
        {
            Dash();
        }
    }

    private void Dash()
    {
        _combat.TryDash();
    }

    public void ApplyDamage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        if (CurrentHealth == 0)
            gameObject.SetActive(false);
    }

    private void Attack()
    {
        int randomAttackIndex = _combat.GetRandomAttackIndex();

        _animator.SetAttackIndex(randomAttackIndex);
    }

    private void Move(Vector3 inputVector)
    {
        _mover.Move(inputVector, _animator.AttackInProgress);
        _animator.SetRunningState(inputVector.sqrMagnitude > 0);
    }

    private Vector3 GetInputAxis()
    {
        return new Vector2(SimpleInput.GetAxis("Horizontal"), SimpleInput.GetAxis("Vertical"));
    }

    public void Restore(float value)
    {
        throw new System.NotImplementedException();
    }
}
