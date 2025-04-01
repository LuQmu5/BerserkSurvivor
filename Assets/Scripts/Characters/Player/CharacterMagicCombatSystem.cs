using System.Collections;
using UnityEngine;

public class CharacterMagicCombatSystem
{
    private ICoroutineRunner _coroutineRunner;
    private Coroutine _attackCooldownRefreshingRoutine;
    private float _currentAttackCooldown;
    private CharacterStats _characterStats;
    private SpellCastingSystem _spellCastingSystem;
    private SpellBookView _spellBookView;

    public bool AttackEnabled => _attackCooldownRefreshingRoutine == null;
    public float AttackCooldown => _currentAttackCooldown;

    public CharacterMagicCombatSystem(ICoroutineRunner coroutineRunner, CharacterStats characterStats, SpellBookView spellBookView)
    {
        _coroutineRunner = coroutineRunner;
        _characterStats = characterStats;
        _spellCastingSystem = new SpellCastingSystem(coroutineRunner, spellBookView);
        _spellBookView = spellBookView;
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

        return true;
    }

    public void PerformAttack()
    {
        _spellCastingSystem.Cast();
        _attackCooldownRefreshingRoutine = _coroutineRunner.StartCoroutine(AttackCooldownRefreshing());
    }

    private IEnumerator AttackCooldownRefreshing()
    {
        _characterStats.TryGetCurrentValueOfStat(StatNames.AttackCooldown, out _currentAttackCooldown);

        while (_currentAttackCooldown > 0) 
        {
            _characterStats.TryGetCurrentValueOfStat(StatNames.AttackSpeed, out float attackSpeed);
            _characterStats.TryGetCurrentValueOfStat(StatNames.AttackCooldown, out float attackCooldown);

            _currentAttackCooldown -= Time.deltaTime + (Time.deltaTime * attackSpeed); // #test_values
            _spellBookView.UpdateCooldownFillAmount(_currentAttackCooldown / attackCooldown);

            if (_currentAttackCooldown <= 0)
                _currentAttackCooldown = 0;

            yield return null;
        }

        _attackCooldownRefreshingRoutine = null;
    }
}
