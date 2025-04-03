using DG.Tweening;
using UnityEngine;

public class HealthPotionItem : Item
{
    [SerializeField] private float _restoringHealthValue = 10;

    private void Start()
    {
        OnDropped();
    }

    protected override void OnDropped()
    {
        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        transform.DOScale(transform.localScale * 1.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public override void OnPickedUp(IItemPicker itemmPicker)
    {
        if (itemmPicker is IHealth health)
        {
            health.Restore(_restoringHealthValue);
        }

        gameObject.SetActive(false);
    }
}
