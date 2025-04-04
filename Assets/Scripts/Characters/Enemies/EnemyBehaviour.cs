using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour, IHealth, ITypeable
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private StatsData _statsData;

    [field: SerializeField] public EnemyType Type { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float CurrentHealth { get; private set; }

    public Enum ObjType => Type;

    private Transform _player;
    private CharacterStats _stats;

    public void Init(Transform player, Vector3 position)
    {
        _player = player;

        _stats = new CharacterStats(_statsData);

        _stats.TryGetCurrentValueOfStat(StatNames.MaxHealth, out float maxHealth);
        _stats.TryGetCurrentValueOfStat(StatNames.MovementSpeed, out float movementSpeed);

        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = movementSpeed;
    }

    private void Update()
    {
        if (_player == null)
        {
            return;
        }

        _agent.SetDestination(_player.position);
    }

    public void ApplyDamage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        if (CurrentHealth == 0)
            gameObject.SetActive(false);
    }

    public void Restore(float value)
    {
        CurrentHealth += value;
    }
}
