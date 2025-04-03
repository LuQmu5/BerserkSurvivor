using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ServiceInstaller : MonoInstaller
{
    private const string ItemsPath = "Prefabs/Items";

    [SerializeField] private Transform _itemsParent;

    public override void InstallBindings()
    {
        Item[] itemPrefabs = Resources.LoadAll<Item>(ItemsPath);

        ItemFactory itemFactory = new ItemFactory(itemPrefabs, _itemsParent);
        Container.BindInstance(itemFactory).AsSingle().NonLazy();
    }
}