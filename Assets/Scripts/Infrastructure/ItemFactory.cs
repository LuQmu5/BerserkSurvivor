using System.Collections.Generic;
using UnityEngine;

public class ItemFactory
{
    private readonly Dictionary<ItemType, Item> _itemPrefabs = new Dictionary<ItemType, Item>(); 

    private Dictionary<ItemType, ObjectPool<Item>> _pools = new Dictionary<ItemType, ObjectPool<Item>>();

    public static ItemFactory Instance { get; private set; }

    public ItemFactory(Item[] itemPrefabsList, Transform parent)
    {
        Instance = this;

        foreach (Item item in itemPrefabsList)
        {
            _itemPrefabs[item.Type] = item;
            _pools[item.Type] = new ObjectPool<Item>(parent);
        }
    }

    public Item GetItem(ItemType type)
    {
        if (_itemPrefabs.ContainsKey(type))
        {
            Item prefab = _itemPrefabs[type];
            return _pools[type].GetObject(prefab.GetComponent<Item>());
        }
        else
        {
            Debug.LogWarning($"Префаб для предмета типа {type} не найден!");
            return null;
        }
    }

    public void ReturnItem(Item item)
    {
        if (_pools.ContainsKey(item.Type))
        {
            _pools[item.Type].ReturnObject(item);
        }
    }
}
