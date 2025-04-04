
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviourFactory<SpellProjectile>
{
    public ProjectileFactory(SpellProjectile[] projectilesPrefabsList, Transform parent) : base(projectilesPrefabsList, parent)
    {
    }
}

public enum ProjectileType
{
    Fireball,
}
