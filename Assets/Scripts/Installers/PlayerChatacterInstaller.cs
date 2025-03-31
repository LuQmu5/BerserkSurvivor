using System;
using UnityEngine;
using Zenject;

public class PlayerChatacterInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerInput>().AsSingle();
        Container.Bind<CharacterStats>().AsSingle();
    }
}
