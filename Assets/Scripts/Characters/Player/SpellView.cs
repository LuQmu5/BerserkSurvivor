using System;
using UnityEngine;

public abstract class SpellView : MonoBehaviour, ITypeable, IPoolable
{
    [field: SerializeField] public SpellViewType Type { get; private set; }

    public Enum ObjType => Type;

    public event Action<IPoolable> OnDisableEvent;

    public abstract void Init(ICaster caster);


    private void OnDisable()
    {
        OnDisableEvent?.Invoke(this);
    }
}
