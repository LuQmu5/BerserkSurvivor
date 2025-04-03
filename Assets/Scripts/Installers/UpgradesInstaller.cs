using UnityEngine;
using Zenject;

public class UpgradesInstaller : MonoInstaller
{
    [SerializeField] private UpgradesView _upgradesView;
    [SerializeField] private UpgradesGetTest _upgradesGetTest;

    public override void InstallBindings()
    {
        Container.Bind<UpgradesManager>().AsSingle();

        Container.BindInstance(_upgradesView).AsSingle();
        Container.BindInstance(_upgradesGetTest).AsSingle();

        Container.BindInterfacesAndSelfTo<UpgradesMediator>().AsSingle().NonLazy();
    }
}
