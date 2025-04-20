using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviourFactory<EnemyBehaviour>
{
    public EnemyFactory(EnemyBehaviour[] itemPrefabsList) : base(itemPrefabsList)
    {
    }
}

public enum EnemyType
{
    Knight
}
