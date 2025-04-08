using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour, IHealth, ITypeable, IPoolable
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private StatsData _statsData;
    [SerializeField] private RagdollController _ragdoll;
    [SerializeField] private Animator _animator;

    [field: SerializeField] public EnemyType Type { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float CurrentHealth { get; private set; }

    public Enum ObjType => Type;

    public Transform Transform => transform;

    private Transform _player;
    private CharacterStats _stats;

    public event Action<IPoolable> OnDisableEvent;

    public void Init(Transform player, Vector3 position)
    {
        _player = player;

        _stats = new CharacterStats(_statsData);

        _stats.TryGetCurrentValueOfStat(StatNames.MaxHealth, out float maxHealth);
        _stats.TryGetCurrentValueOfStat(StatNames.MovementSpeed, out float movementSpeed);

        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;

        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = Random.Range(movementSpeed / 1.5f, movementSpeed * 1.2f); // # config
        _agent.angularSpeed = Random.Range(movementSpeed * 15, movementSpeed * 20); // # config
        _agent.isStopped = false;

        _ragdoll.SetRagdollState(false);
        _animator.enabled = true;
    }

    private void Update()
    {
        if (_player == null)
        {
            return;
        }

        _agent.SetDestination(_player.position);
    }

    private void OnDisable()
    {
        OnDisableEvent?.Invoke(this);
    }

    public void ApplyDamage(float amount)
    {
        bool isCriticalHit = amount > MaxHealth / 2;
        CurrentHealth -= amount;

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        if (CurrentHealth == 0)
            Death(isCriticalHit);
    }

    private void Death(bool isCriticalHit)
    {
        _player = null;
        _agent.isStopped = true;
        _animator.enabled = false;

        float criticalHitPushPower = 200;
        float noCriticalHitPushPower = 10;

        _ragdoll.SetRagdollState(true, pushPower: isCriticalHit? criticalHitPushPower : noCriticalHitPushPower); // #config

        StartCoroutine(DelayDeactivating());
    }

    private IEnumerator DelayDeactivating()
    {
        yield return new WaitForSeconds(1);

        gameObject.SetActive(false);
    }

    public void Restore(float value)
    {
        CurrentHealth += value;
    }
}
