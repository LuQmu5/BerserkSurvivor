using DG.Tweening;
using UnityEngine;

public class HealthPotionItem : Item
{
    [SerializeField] private float _restoringHealthValue = 10;

    public override void OnPickedUp(IItemPicker itemmPicker)
    {
        if (itemmPicker is IHealth health)
        {
            health.Restore(_restoringHealthValue);
        }
        
        base.OnPickedUp(itemmPicker);
    }
}
