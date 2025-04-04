using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component, IPoolable
{
    private Stack<T> _pool = new Stack<T>();
    private Transform _parentTransform;

    public ObjectPool(Transform parent)
    {
        _parentTransform = parent;
    }

    public T GetObject(T prefab)
    {
        T obj;

        if (_pool.Count > 0)
        {
            obj = _pool.Pop();
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = UnityEngine.Object.Instantiate(prefab, _parentTransform);
            obj.OnDisableEvent += ReturnObject; // Подписываемся на событие отключения
        }

        return obj;
    }

    private void ReturnObject(IPoolable item)
    {
        if (item is T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }
    }
}
