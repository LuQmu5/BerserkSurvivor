using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

public enum AnimationNames
{
    None,
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
    private List<AnimationNames> _attackAnimations;

    public bool AttackAnimationInProgress { get; private set; } = false;

    public event Action CurrentAnimationPerformed;
    public event Action CurrentAnimationCanceled;

    public CharacterView(Animator animator)
    {
        _animator = animator;

        _attackAnimations = new List<AnimationNames>()
        {
            AnimationNames.SimpleAttack,
            AnimationNames.EarthSlam,
            AnimationNames.Buff,
            AnimationNames.HeavySlowAttack ,
            AnimationNames.Channeling,
            AnimationNames.EarthSlamChanneling,
        };
    }

    public bool TryStartAttackAnimation(AnimationNames name)
    {
        if (AttackAnimationInProgress)
            return false;

        _animator.Play(name.ToString());
        AttackAnimationInProgress = true;

        return true;
    }

    public void SetRunningState(bool isRunning)
    {
        _animator.SetBool(IsRunning, isRunning);
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
    public void CallEndOfAttackAnimation(bool isCanceled)
    {
        AttackAnimationInProgress = false;
        _animator.SetTrigger(BreakAttackAnimationTrigger);

        if (isCanceled)
            CurrentAnimationCanceled?.Invoke();
        else
            CurrentAnimationPerformed?.Invoke();
    }
}
