
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviourFactory<SpellProjectile>
{
    public ProjectileFactory(SpellProjectile[] projectilesPrefabsList) : base(projectilesPrefabsList)
    {
    }
}

public enum ProjectileType
{
    Fireball,
}
