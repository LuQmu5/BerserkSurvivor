using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class CharacterMagicCombatSystem
{
    private ICoroutineRunner _coroutineRunner;
    private float _currentAttackCooldown;
    private CharacterStats _characterStats;
    private SpellCastingSystem _spellCastingSystem;
    private SpellBookView _spellBookView;
    private ICharacterView _view;
    private PlayerInput _input;

    private Coroutine _attackCooldownRefreshingCoroutine;

    public SpellData ActiveSpellData => _spellCastingSystem.CurrentActiveSpell.Data;

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

    public bool TryStartAttackPerform()
    {
        Debug.LogWarning("Try start to perform attack");
        Debug.LogWarning($"Attack on cooldown: {_attackCooldownRefreshingCoroutine != null}");
        Debug.LogWarning($"No Active Spell Data Is Set: {ActiveSpellData == null}");
        Debug.LogWarning($"Attack animation in progress: {_view.AttackAnimationInProgress}");

        if (_attackCooldownRefreshingCoroutine != null) // tyt trabla
            return false;

        if (ActiveSpellData == null)
            return false;

        if (_view.TryStartAttackAnimation(ActiveSpellData.CastAnimationName) == false)
            return false;

        Debug.LogWarning("Attack started");
        _view.CurrentAnimationPerformed += OnAnimationPerformed;
        _view.CurrentAnimationCanceled += OnAnimationCanceled;

        return true;
    }

    private void OnAnimationCanceled()
    {
        Debug.LogWarning("Attack Canceled!");

        _view.CurrentAnimationPerformed -= OnAnimationPerformed;
        _view.CurrentAnimationCanceled -= OnAnimationCanceled;
    }

    private void OnAnimationPerformed()
    {
        Debug.LogWarning("Attack Performed!");

        _view.CurrentAnimationPerformed -= OnAnimationPerformed;
        _view.CurrentAnimationCanceled -= OnAnimationCanceled;

        RunAttackCooldown();
        _spellCastingSystem.Cast();
    }

    private void RunAttackCooldown()
    {
        if (_attackCooldownRefreshingCoroutine != null)
            _coroutineRunner.StopCoroutine(_attackCooldownRefreshingCoroutine);

        _attackCooldownRefreshingCoroutine = _coroutineRunner.StartCoroutine(AttackCooldownRefreshing());
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

        _attackCooldownRefreshingCoroutine = null;
    }
}
