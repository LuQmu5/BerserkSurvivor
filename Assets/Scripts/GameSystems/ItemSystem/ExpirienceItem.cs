using UnityEngine;

public class ExpirienceItem : Item
{
    [SerializeField] private float _gainingExpirienceValue = 10;

    public override void OnPickedUp(IItemPicker itemmPicker)
    {
        if (itemmPicker is ILevelable levelable)
        {
            levelable.AddExpirience(_gainingExpirienceValue);
        }

        base.OnPickedUp(itemmPicker);
    }
}