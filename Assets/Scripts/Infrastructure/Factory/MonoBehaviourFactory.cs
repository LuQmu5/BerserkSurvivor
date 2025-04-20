
using System;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourFactory<T> : IFactory<T> where T : Component, IPoolable
{
    private readonly Dictionary<Enum, T> _itemPrefabs = new Dictionary<Enum, T>();
    private Dictionary<Enum, ObjectPool<T>> _pools = new Dictionary<Enum, ObjectPool<T>>();

    public MonoBehaviourFactory(T[] itemPrefabsList)
    {
        foreach (T item in itemPrefabsList)
        {
            if (item is ITypeable typeable)
            {
                _itemPrefabs[typeable.ObjType] = item;
                _pools[typeable.ObjType] = new ObjectPool<T>();
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
}
