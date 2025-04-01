using System;

public interface ICharacterView
{
    bool AttackAnimationInProgress { get; }

    event Action CurrentAnimationPerformed;
    event Action CurrentAnimationCanceled;

    void CallEndOfAttackAnimation(bool isBreaked);
    bool TryStartAttackAnimation(AnimationNames name);
    void SetAttackSpeedMultiplier(float value);
    void SetRunningState(bool state);
}