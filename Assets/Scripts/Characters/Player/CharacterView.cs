using UnityEngine;
using System;
using Random = UnityEngine.Random;

public enum AnimationNames
{
    Idle,
    Run,
    SimpleAttack,
    HeavySlowAttack,
    Channeling,
    Buff,
    EarthSlam,
    EarthSlamChanneling
}

public class CharacterView : ICharacterView
{
    private const string IsRunning = nameof(IsRunning);
    private const string AttackVariation = nameof(AttackVariation);
    private const string AttackSpeedMultiplier = nameof(AttackSpeedMultiplier);
    private const string BreakAttackAnimationTrigger = nameof(BreakAttackAnimationTrigger);

    private Animator _animator;

    public bool AttackInProgress => IsAttackAnimationActive();

    public event Action<ICharacterView> CurrentAnimationPerformed;

    public CharacterView(Animator animator)
    {
        _animator = animator;
    }

    private bool IsAttackAnimationActive()
    {
        AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);

        if (clipInfo.Length > 0)
        {
            string clipName = clipInfo[0].clip.name;
            return clipName.Contains(AttackVariation);
        }

        return false;
    }

    public void StartAnimation(AnimationNames name)
    {
        _animator.Play(name.ToString());
    }

    public void SetRunningState(bool isRunning)
    {
        _animator.SetBool(IsRunning, isRunning);

        if (isRunning && IsAttackAnimationActive())
        {
            CallEndOfAttackAnimation(isBreaked: true);
        }
    }

    public void SetAttackSpeedMultiplier(float value)
    {
        // #test_values
        float minValue = 0.1f;
        float maxValue = 3.5f;
        value = Mathf.Clamp(value, minValue, maxValue);

        _animator.SetFloat(AttackSpeedMultiplier, value);
    }

    /// <summary>
    /// calling in animations events, maybe make with routines and timers
    /// </summary>
    public void CallEndOfAttackAnimation(bool isBreaked)
    {
        Debug.Log("Break: " + isBreaked);

        _animator.SetBool(IsRunning, false);
        _animator.Play(AnimationNames.Idle.ToString());
        _animator.SetTrigger(BreakAttackAnimationTrigger);

        if (isBreaked == false)
            CurrentAnimationPerformed?.Invoke(this);
    }

    public float GetCurrentAnimationLength() => _animator.GetCurrentAnimatorClipInfo(0).Length;
}
