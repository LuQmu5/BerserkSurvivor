using UnityEngine;

public abstract class SpellProjectile : MonoBehaviour
{
    [SerializeField] protected LayerMask HittableLayer;

    public abstract void Init(ICaster caster);
}
