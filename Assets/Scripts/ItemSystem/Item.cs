using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string _itemName;

    public string Name => _itemName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IItemPicker itemPicker))
        {
            itemPicker.PickUp(this);
        }
    }
}

public interface IItemPicker
{
    public void PickUp(Item item);
}
