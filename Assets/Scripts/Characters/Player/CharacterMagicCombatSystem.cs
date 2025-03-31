using System.Collections;
using UnityEngine;

public class CharacterMagicCombatSystem
{
    private ICoroutineRunner _coroutineRunner;
    private Coroutine _attackCooldownRefreshingRoutine;
    private float _attackCooldown;
    private CharacterStats _characterStats;
    private SpellCastingSystem _spellCastingSystem;

    public bool AttackEnabled => _attackCooldownRefreshingRoutine == null;
    public float AttackCooldown => _attackCooldown;

    public CharacterMagicCombatSystem(ICoroutineRunner coroutineRunner, CharacterStats characterStats, SpellBookView spellBookView)
    {
        _coroutineRunner = coroutineRunner;
        _characterStats = characterStats;
        _spellCastingSystem = new SpellCastingSystem(coroutineRunner, spellBookView);
    }

    public bool TryActivateSpell()
    {
        return _spellCastingSystem.TryActiveSpell();
    }

    public bool TryStartAttack()
    {
        if (_attackCooldownRefreshingRoutine != null)
            return false;

        if (_spellCastingSystem.CurrentActiveSpell == null)
            return false;

        _attackCooldownRefreshingRoutine = _coroutineRunner.StartCoroutine(AttackCooldownRefreshing());
        _spellCastingSystem.Cast();

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
