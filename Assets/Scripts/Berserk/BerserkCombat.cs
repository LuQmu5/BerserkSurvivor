using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BerserkCombat : MonoBehaviour
{
    private const string Attack = nameof(Attack);

    [SerializeField] private Sword _sword;

    private Collider _swordCollider;
    private int _attacksCount = 5;

    private void Awake()
    {
        _swordCollider = _sword.GetComponent<Collider>();
    }

    public void EnableCollider()
    {
        _swordCollider.enabled = true;
    }

    public void DisableCollider()
    {
        _swordCollider.enabled = false;
    }

    public int GetRandomAttackIndex()
    {
        return Random.Range(0, _attacksCount);
    }
}
