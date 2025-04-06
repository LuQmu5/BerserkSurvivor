using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FactoryInstaller : MonoInstaller
{
    private const string ItemsPath = "Prefabs/Items";
    private const string EnemiesPath = "Prefabs/Enemies";
    private const string SpellViewsPath = "Prefabs/Spells";

    public override void InstallBindings()
    {
        InitItemFactory();
        InitEnemiesFactory();
        InitSpellProjectilesFactory();
    }

    private void InitSpellProjectilesFactory()
    {
        SpellView[] spellProjectiles = Resources.LoadAll<SpellView>(SpellViewsPath);

        SpellsViewFactory projectileFactory = new SpellsViewFactory(spellProjectiles);
        Container.BindInstance(projectileFactory).AsSingle().NonLazy();
    }

    private void InitEnemiesFactory()
    {
        EnemyBehaviour[] enemiesPrefabs = Resources.LoadAll<EnemyBehaviour>(EnemiesPath);

        EnemyFactory enemiesFactory = new EnemyFactory(enemiesPrefabs);
        Container.BindInstance(enemiesFactory).AsSingle().NonLazy();
    }

    private void InitItemFactory()
    {
        Item[] itemPrefabs = Resources.LoadAll<Item>(ItemsPath);

        ItemFactory itemFactory = new ItemFactory(itemPrefabs);
        Container.BindInstance(itemFactory).AsSingle().NonLazy();
    }
}