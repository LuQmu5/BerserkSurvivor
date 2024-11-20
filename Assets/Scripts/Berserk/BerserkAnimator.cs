using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BerserkAnimator : MonoBehaviour
{
    private const string AttackIndex = nameof(AttackIndex);
    private const string IsRunning = nameof(IsRunning);
    private const int NoAttackIndex = -1;

    [SerializeField] private Animator _animator;

    private int _currentAttackIndex = -1;

    public bool AttackInProgress => _currentAttackIndex != NoAttackIndex;

    public void SetAttackIndex(int index)
    {
        _currentAttackIndex = index;
        _animator.SetInteger(AttackIndex, index);
    }

    public void ResetAttackIndex()
    {
        _currentAttackIndex = NoAttackIndex;
        _animator.SetInteger(AttackIndex, NoAttackIndex);
    }

    public void SetRunningState(bool state)
    {
        _animator.SetBool(IsRunning, state);
    }
}
