using UnityEngine;
using Zenject;

public class ItemFactory : MonoBehaviourFactory<Item>
{
    public ItemFactory(Item[] itemPrefabsList, Transform parent)
        : base(itemPrefabsList, parent)
    {
        Debug.Log("Item Factory");
        Debug.Log(itemPrefabsList.Length + ": Items count");
    }
}
