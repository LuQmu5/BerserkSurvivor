using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BerserkCombat : MonoBehaviour
{
    private const string Attack = nameof(Attack);

    [SerializeField] private Sword _sword;
    [SerializeField] private float _dashPower = 40;
    [SerializeField] private CharacterController _controller;

    private Collider _swordCollider;
    private Coroutine _dashCoroutine;

    private int _attackVariationsCount = 5;
    private float _dashDuration = 0.5f;
    private float _dashCooldown = 5;
    private float _currentDashCooldown = 0;

    public bool ProcessingDash => _dashCoroutine != null;

    private void Awake()
    {
        _swordCollider = _sword.GetComponent<Collider>();
    }

    /// <summary>
    /// calling in animation event
    /// </summary>
    public void EnableCollider()
    {
        _swordCollider.enabled = true;
    }

    /// <summary>
    /// calling in animation event
    /// </summary>
    public void DisableCollider()
    {
        _swordCollider.enabled = false;
    }

    public int GetRandomAttackIndex()
    {
        return Random.Range(0, _attackVariationsCount);
    }

    public bool TryDash()
    {
        if (_currentDashCooldown > 0)
            return false;

        _dashCoroutine = StartCoroutine(Dashing());
        StartCoroutine(DashCooldowning());

        return true;
    }

    private IEnumerator Dashing()
    {
        Debug.Log("Start dash");

        float duration = _dashDuration;
        _currentDashCooldown = _dashCooldown;

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            _controller.Move(new Vector3(transform.forward.x, 0, transform.forward.y) * _dashPower * Time.deltaTime);

            Debug.Log("Дамаг по врагам + рассталкивание");

            yield return null;
        }

        _dashCoroutine = null;
    }
    private IEnumerator DashCooldowning()
    {
        while (_currentDashCooldown > 0)
        {
            _currentDashCooldown -= Time.deltaTime;

            yield return null;
        }

        _currentDashCooldown = 0;
    }
}
