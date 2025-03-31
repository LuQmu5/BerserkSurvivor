using System.Collections;
using UnityEngine;

public class CharacterMagicCombatSystem
{
    private ICoroutineRunner _coroutineRunner;
    private Coroutine _attackCooldownRefreshingRoutine;
    private float _attackCooldown;
    private CharacterStats _characterStats;

    public bool AttackEnabled => _attackCooldownRefreshingRoutine == null;
    public float AttackCooldown => _attackCooldown;

    public CharacterMagicCombatSystem(ICoroutineRunner coroutineRunner, CharacterStats characterStats)
    {
        _coroutineRunner = coroutineRunner;
        _characterStats = characterStats;
    }

    public bool TryStartAttack()
    {
        if (_attackCooldownRefreshingRoutine != null)
            return false;

        _attackCooldownRefreshingRoutine = _coroutineRunner.StartCoroutine(AttackCooldownRefreshing());

        return true;
    }

    private IEnumerator AttackCooldownRefreshing()
    {
        _attackCooldown = _characterStats.GetStatValue(StatNames.AttackCooldown);

        while (_attackCooldown > 0) 
        {
            _attackCooldown -= Time.deltaTime * _characterStats.GetStatValue(StatNames.AttackSpeed);

            if (_attackCooldown <= 0)
                _attackCooldown = 0;

            yield return null;
        }

        _attackCooldownRefreshingRoutine = null;
    }
}
