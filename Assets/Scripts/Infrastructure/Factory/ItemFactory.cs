using UnityEngine;
using Zenject;

public class ItemFactory : MonoBehaviourFactory<Item>
{
    public ItemFactory(Item[] itemPrefabsList)
        : base(itemPrefabsList)
    {
    }
}
