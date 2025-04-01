using System;

public interface ICharacterView
{
    bool AttackInProgress { get; }

    event Action<ICharacterView> CurrentAnimationPerformed;

    void CallEndOfAttackAnimation(bool isBreaked);
    void StartAnimation(AnimationNames name);
    void SetAttackSpeedMultiplier(float value);
    void SetRunningState(bool state);
    float GetCurrentAnimationLength();
}