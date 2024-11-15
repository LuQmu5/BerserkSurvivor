using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BerserkAnimator : MonoBehaviour
{
    private const string AttackIndex = nameof(AttackIndex);
    private const string IsRunning = nameof(IsRunning);
    private const int BaseAttackIndex = -1;

    [SerializeField] private Animator _animator;

    public void SetAttackIndex(int index)
    {
        _animator.SetInteger(AttackIndex, index);
    }

    public void ResetAttackIndex()
    {
        _animator.SetInteger(AttackIndex, BaseAttackIndex);
    }

    public void SetRunningState(bool state)
    {
        _animator.SetBool(IsRunning, state);
    }
}
