
using System;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourFactory<T> : IFactory<T> where T : Component
{
    private readonly Dictionary<Enum, T> _itemPrefabs = new Dictionary<Enum, T>();
    private Dictionary<Enum, ObjectPool<T>> _pools = new Dictionary<Enum, ObjectPool<T>>();

    public static MonoBehaviourFactory<T> Instance { get; private set; }

    public MonoBehaviourFactory(T[] itemPrefabsList, Transform parent)
    {
        Instance = this;

        foreach (T item in itemPrefabsList)
        {
            Debug.Log(item.name);

            if (item is ITypeable typeable)
            {

                Debug.Log("If for this name");
                _itemPrefabs[typeable.ObjType] = item;
                _pools[typeable.ObjType] = new ObjectPool<T>(parent);
            }
        }
    }

    public T GetItem(Enum type)
    {
        if (_itemPrefabs.ContainsKey(type))
        {
            T prefab = _itemPrefabs[type];
            return _pools[type].GetObject(prefab);
        }
        else
        {
            Debug.LogWarning($"Prefab for item type {type} not found!");
            return null;
        }
    }

    public void ReturnItem(T item)
    {
        if (item is ITypeable typeable && _pools.ContainsKey(typeable.ObjType))
        {
            _pools[typeable.ObjType].ReturnObject(item);
        }
    }
}
