using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class CharacterMagicCombatSystem
{
    private ICoroutineRunner _coroutineRunner;
    private Coroutine _attackCooldownRefreshingRoutine;
    private float _currentAttackCooldown;
    private CharacterStats _characterStats;
    private SpellCastingSystem _spellCastingSystem;
    private SpellBookView _spellBookView;
    private Coroutine _attackPerformProcessingCoroutine;
    private ICharacterView _view;
    private PlayerInput _input;

    private Coroutine _attackCooldownRefreshingCoroutine;

    public SpellData ActiveSpellData => _spellCastingSystem.CurrentActiveSpell.Data;
    public bool AttackEnabled => _attackCooldownRefreshingRoutine == null;
    public float AttackCooldown => _currentAttackCooldown;
    public bool CanAttack => IsAttackPossible();

    public CharacterMagicCombatSystem(ICoroutineRunner coroutineRunner, CharacterStats characterStats, SpellBookView spellBookView, ICharacterView view, PlayerInput input)
    {
        _coroutineRunner = coroutineRunner;
        _characterStats = characterStats;
        _spellCastingSystem = new SpellCastingSystem(coroutineRunner, spellBookView);
        _spellBookView = spellBookView;
        _input = input;
        _view = view;
    }

    public bool TryActivateSpell()
    {
        return _spellCastingSystem.TryActiveSpell();
    }

    public bool IsAttackPossible()
    {
        if (_attackCooldownRefreshingRoutine != null)
            return false;

        if (_spellCastingSystem.CurrentActiveSpell == null)
            return false;

        return true;
    }

    public void StartAttackPerform()
    {
        if (_attackPerformProcessingCoroutine != null)
            _coroutineRunner.StopCoroutine(_attackPerformProcessingCoroutine);

        _attackPerformProcessingCoroutine = _coroutineRunner.StartCoroutine(AttackPerformProcessing());
        _view.CurrentAnimationPerformed += PerformAttack; // много подписок
        Debug.LogWarning("Sub to perf att");
    }

    private IEnumerator AttackPerformProcessing()
    {
        yield return new WaitForEndOfFrame();

        float totalProcessLength = _view.GetCurrentAnimationLength();
        float currentProcessLength = totalProcessLength;

        while (currentProcessLength > 0)
        {
            if (_input.Combat.Attack.IsPressed())
            {
                currentProcessLength = Mathf.Clamp(currentProcessLength - Time.deltaTime, 0, currentProcessLength);

                yield return null;
            }
            else
            {
                if (_input.Combat.Attack.IsInProgress())
                    continue;

                Debug.Log("Breaked Pressing");
                _attackPerformProcessingCoroutine = null;
                currentProcessLength = 0;

                _view.CallEndOfAttackAnimation(isBreaked: true);
                _view.CurrentAnimationPerformed -= PerformAttack;

                yield break;
            }

            yield return null;
            // Debug.Log($"Progress Pressing: {currentProcessLength / totalProcessLength}");
        }
    }

    private void PerformAttack(ICharacterView viewCaller)
    {
        Debug.LogWarning("Attack Performed!");

        EndAttackProcessing();
        RunAttackCooldown();

        _spellCastingSystem.Cast();
        viewCaller.CurrentAnimationPerformed -= PerformAttack;
    }

    private void RunAttackCooldown()
    {
        if (_attackCooldownRefreshingCoroutine != null)
            _coroutineRunner.StopCoroutine(_attackCooldownRefreshingCoroutine);

        _attackCooldownRefreshingCoroutine = _coroutineRunner.StartCoroutine(AttackCooldownRefreshing());
    }

    private void EndAttackProcessing()
    {
        if (_attackPerformProcessingCoroutine != null)
            _coroutineRunner.StopCoroutine(_attackPerformProcessingCoroutine);
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
