
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory
{
    private readonly Dictionary<EnemyType, EnemyBehaviour> _itemPrefabs = new Dictionary<EnemyType, EnemyBehaviour>();
    private Dictionary<EnemyType, ObjectPool<EnemyBehaviour>> _pools = new Dictionary<EnemyType, ObjectPool<EnemyBehaviour>>();

    public static EnemyFactory Instance { get; private set; }

    public EnemyFactory(EnemyBehaviour[] itemPrefabsList, Transform parent)
    {
        Instance = this;

        foreach (EnemyBehaviour item in itemPrefabsList)
        {
            _itemPrefabs[item.Type] = item; 
            _pools[item.Type] = new ObjectPool<EnemyBehaviour>(parent);
        }
    }

    public EnemyBehaviour GetItem(EnemyType type)
    {
        if (_itemPrefabs.ContainsKey(type))
        {
            EnemyBehaviour prefab = _itemPrefabs[type];
            return _pools[type].GetObject(prefab);
        }
        else
        {
            Debug.LogWarning($"Prefab for item type {type} not found!");
            return null;
        }
    }

    public void ReturnItem(EnemyBehaviour item)
    {
        if (_pools.ContainsKey(item.Type))
        {
            _pools[item.Type].ReturnObject(item);
        }
    }
}

public enum EnemyType
{
    Knight,
    Zombie,
}

public interface IFactory<T>
{
    T GetItem(Enum type);
    void ReturnItem(T item);
}