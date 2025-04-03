using DG.Tweening;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    private Collider _collider;

    public float rotationSpeed = 50f; // Скорость вращения
    public float pulseScaleFactor = 1.2f; // Насколько увеличивается объект
    public float pulseDuration = 1f; // Длительность пульсации

    private Vector3 originalScale;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IItemPicker itemPicker) && _collider.enabled)
        {
            itemPicker.PickUp(this);
            _collider.enabled = false;
        }
    }

    protected abstract void OnDropped();
    public abstract void OnPickedUp(IItemPicker picker);
}
