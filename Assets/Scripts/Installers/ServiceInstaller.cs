using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ServiceInstaller : MonoInstaller
{
    private const string ItemsPath = "Prefabs/Items";
    private const string EnemiesPath = "Prefabs/Enemies";

    [SerializeField] private Transform _itemsParent;
    [SerializeField] private Transform _enemiesParent;

    public override void InstallBindings()
    {
        InitItemFactory();
        InitEnemiesFactory();
    }

    private void InitEnemiesFactory()
    {
        EnemyBehaviour[] enemiesPrefabs = Resources.LoadAll<EnemyBehaviour>(EnemiesPath);

        EnemyFactory enemiesFactory = new EnemyFactory(enemiesPrefabs, _enemiesParent);
        Container.BindInstance(enemiesFactory).AsSingle().NonLazy();
    }

    private void InitItemFactory()
    {
        Item[] itemPrefabs = Resources.LoadAll<Item>(ItemsPath);

        ItemFactory itemFactory = new ItemFactory(itemPrefabs, _itemsParent);
        Container.BindInstance(itemFactory).AsSingle().NonLazy();
    }
}