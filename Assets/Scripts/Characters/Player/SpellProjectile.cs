using System;
using UnityEngine;

public abstract class SpellProjectile : MonoBehaviour, ITypeable, IPoolable
{
    [field: SerializeField] public ProjectileType Type { get; private set; }

    public Enum ObjType => Type;

    public event Action<IPoolable> OnDisableEvent;

    public abstract void Init(ICaster caster);


    private void OnDisable()
    {
        OnDisableEvent?.Invoke(this);
    }
}
