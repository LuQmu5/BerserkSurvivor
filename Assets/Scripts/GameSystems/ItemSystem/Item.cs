using UnityEngine;

public abstract class Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IItemPicker itemPicker))
        {
            itemPicker.PickUp(this);
        }
    }

    protected abstract void OnDropped();
    public abstract void OnPickedUp(IItemPicker picker);
}
