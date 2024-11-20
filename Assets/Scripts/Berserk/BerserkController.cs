using UnityEngine;
using Zenject;

public class BerserkController : MonoBehaviour, IHealth
{
    [SerializeField] private BerserkAnimator _animator;
    [SerializeField] private BerserkCombat _combat;
    [SerializeField] private BerserkMover _mover;

    private PlayerInput _input;

    public float MaxHealth { get; private set; } = 10;
    public float CurrentHealth { get; private set; }

    [Inject]
    public void Construct(PlayerInput input)
    {
        _input = input;
        _input.Enable();

        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (_animator.AttackInProgress == false)
        {
            Move();       
        }

        if (_input.Combat.Attack.triggered)
        {
            Attack();
        }
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

        _mover.RotateToClick();
        _animator.SetAttackIndex(randomAttackIndex);
    }

    private void Move()
    {
        Vector3 inputVector = GetInputAxis();
        _mover.Move(inputVector);
        _animator.SetRunningState(inputVector.sqrMagnitude > 0);
    }

    private Vector3 GetInputAxis()
    {
        return new Vector2(SimpleInput.GetAxis("Horizontal"), SimpleInput.GetAxis("Vertical"));
    }
}
