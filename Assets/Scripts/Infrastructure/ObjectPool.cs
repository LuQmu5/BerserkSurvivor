using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private Stack<T> _pool = new Stack<T>();
    private Transform _parentTransform;

    public ObjectPool(Transform parent)
    {
        _parentTransform = parent;
    }

    public T GetObject(T prefab)
    {
        if (_pool.Count > 0)
        {
            T obj = _pool.Pop();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            T obj = Object.Instantiate(prefab, _parentTransform);
            return obj;
        }
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Push(obj);
    }
}
