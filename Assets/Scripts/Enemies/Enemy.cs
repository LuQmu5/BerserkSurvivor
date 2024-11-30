using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHealth
{
    [SerializeField] private Transform _player;
    [SerializeField] private NavMeshAgent _agent;

    public float MaxHealth { get; private set; } = 100;
    public float CurrentHealth { get; private set; }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        _agent.SetDestination(_player.position);
    }

    public void ApplyDamage(float amount)
    {
        CurrentHealth -= amount;
        print(CurrentHealth);

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        if (CurrentHealth == 0)
            gameObject.SetActive(false);
    }
}
