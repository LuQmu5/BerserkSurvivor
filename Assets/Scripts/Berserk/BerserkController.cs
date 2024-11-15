using UnityEngine;
using Zenject;

public class BerserkController : MonoBehaviour, IHealth
{
    [SerializeField] private BerserkAnimator _animator;
    [SerializeField] private BerserkCombat _combat;
    [SerializeField] private BerserkMover _mover;

    [field:SerializeField] public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        Vector3 inputVector = GetInputAxis();

        _mover.Move(inputVector);
    }

    public void ApplyDamage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        if (CurrentHealth == 0)
            gameObject.SetActive(false);
    }

    private Vector3 GetInputAxis()
    {
        return new Vector2(SimpleInput.GetAxis("Horizontal"), SimpleInput.GetAxis("Vertical"));
    }
}
