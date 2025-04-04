using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour, IHealth
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private CharacterStats _stats;

    [field: SerializeField] public EnemyType Type { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; } = 100;
    [field: SerializeField] public float CurrentHealth { get; private set; }

    private Transform _player;

    public void Init(Transform player)
    {
        _player = player;
        _stats.TryGetCurrentValueOfStat(StatNames.MaxHealth, out float maxHealth);
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        _agent = GetComponent<NavMeshAgent>();
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
        
    }
}
