using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHealth
{
    [SerializeField] private Transform _player;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private BloodVFX _bloodVFX;

    [field: SerializeField] public float MaxHealth { get; private set; } = 100;
    [field: SerializeField] public float CurrentHealth { get; private set; }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<CharacterBehaviour>().transform;
            return;
        }

        _agent.SetDestination(_player.position);
    }

    public void ApplyDamage(float amount)
    {
        CurrentHealth -= amount;
        print(CurrentHealth);

        Instantiate(_bloodVFX, transform.RandomOffset(0.5f, 1, 0.5f), Quaternion.identity);

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        if (CurrentHealth == 0)
            gameObject.SetActive(false);
    }

    public void Restore(float value)
    {
        
    }
}
