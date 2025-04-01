using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class CharacterView
{
    private const string IsRunning = nameof(IsRunning);
    private const string AttackVariation = nameof(AttackVariation);
    private const string AttackSpeedMultiplier = nameof(AttackSpeedMultiplier);

    private Animator _animator;
    private int _attackVariationsCount;

    public bool AttackInProgress => IsAttackAnimationIsActive();

    public CharacterView(Animator animator)
    {
        _animator = animator;
        _attackVariationsCount = GetAttackVariationsCount();
        Debug.Log(_attackVariationsCount);
    }

    private bool IsAttackAnimationIsActive()
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

    public void SetRunningState(bool state)
    {
        _animator.SetBool(IsRunning, state);
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
}
