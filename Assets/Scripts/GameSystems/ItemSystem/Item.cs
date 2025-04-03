using DG.Tweening;
using UnityEngine;
using Zenject.SpaceFighter;

public abstract class Item : MonoBehaviour
{
    private Collider _collider;

    public float jumpHeight = 0.5f;  // ������ �������������
    public float jumpDuration = 0.3f; // ������������ �������������
    public float rotationSpeed = 50f; // �������� ��������
    public float pulseScaleFactor = 1.2f; // ��������� ������������� ������
    public float pulseDuration = 1f; // ������������ ���������

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public virtual void OnDropped()
    {
        transform.DOLocalMoveY(transform.position.y + jumpHeight, jumpDuration)
         .SetEase(Ease.OutQuad)  // ������� ������ � �����
         .OnComplete(() =>
             transform.DOLocalMoveY(transform.position.y - jumpHeight, jumpDuration)
                      .SetEase(Ease.InQuad)  // ������� ����������� ����
         );

        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        transform.DOScale(transform.localScale * 1.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IItemPicker itemPicker) && _collider.enabled)
        {
            itemPicker.PickUp(this);
            _collider.enabled = false;
        }
    }

    public abstract void OnPickedUp(IItemPicker picker);
}
