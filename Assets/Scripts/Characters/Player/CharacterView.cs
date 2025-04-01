using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class CharacterView : ICharacterView
{
    private const string IsRunning = nameof(IsRunning);
    private const string AttackVariation = nameof(AttackVariation);
    private const string AttackSpeedMultiplier = nameof(AttackSpeedMultiplier);
    private const string BreakAttackAnimationTrigger = nameof(BreakAttackAnimationTrigger);

    private Animator _animator;
    private int _attackVariationsCount;

    public bool AttackInProgress => IsAttackAnimationActive();

    public event Action<ICharacterView> CurrentAnimationPerformed;

    public CharacterView(Animator animator)
    {
        _animator = animator;
        _attackVariationsCount = GetAttackVariationsCount();
        Debug.Log(_attackVariationsCount);
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

    public void PlayAttackAnimation()
    {
        int index = Random.Range(0, _attackVariationsCount);
        _animator.Play(AttackVariation + index);
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

    private int GetAttackVariationsCount()
    {
        RuntimeAnimatorController controller = _animator.runtimeAnimatorController;
        int result = 0;

        foreach (AnimationClip clip in controller.animationClips)
        {
            if (clip.name.Contains(AttackVariation))
            {
                result++;
            }
        }

        return result;
    }

    /// <summary>
    /// calling in animations events, maybe make with routines and timers
    /// </summary>
    public void CallEndOfAttackAnimation(bool isBreaked)
    {
        _animator.SetTrigger(BreakAttackAnimationTrigger);

        if (isBreaked == false)
            CurrentAnimationPerformed?.Invoke(this);
    }
}
