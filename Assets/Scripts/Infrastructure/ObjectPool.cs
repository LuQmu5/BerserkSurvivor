using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private Stack<T> _pool = new Stack<T>();  // Стек для объектов
    private Transform _parentTransform;       // Родитель для объектов в пуле

    public ObjectPool(Transform parent)
    {
        _parentTransform = parent;
    }

    // Получаем объект из пула или создаем новый
    public T GetObject(T prefab)
    {
        if (_pool.Count > 0)
        {
            T obj = _pool.Pop();
            obj.gameObject.SetActive(true);  // Активируем объект
            return obj;
        }
        else
        {
            // Создаем новый объект, если пул пуст
            T obj = Object.Instantiate(prefab, _parentTransform);
            return obj;
        }
    }

    // Возвращаем объект в пул
    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);  // Деактивируем объект
        _pool.Push(obj);  // Добавляем в пул
    }
}