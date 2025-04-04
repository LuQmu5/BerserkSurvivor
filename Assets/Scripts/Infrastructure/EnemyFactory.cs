using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviourFactory<EnemyBehaviour>
{
    public EnemyFactory(EnemyBehaviour[] itemPrefabsList, Transform parent) : base(itemPrefabsList, parent)
    {
    }
}

public enum EnemyType
{
    Knight
}
