using UnityEngine;

public class SpeedItem : Item
{

    [SerializeField] private float _buffBonus = 2;
    [SerializeField] private float _buffDuration = 5;

    public override void OnPickedUp(IItemPicker itemmPicker)
    {
        if (itemmPicker is ICharacterStats characterStats)
        {
            characterStats.AddTemporaryMultiplier(StatNames.MovementSpeed, _buffBonus, _buffDuration);
        }

        base.OnPickedUp(itemmPicker);
    }
}