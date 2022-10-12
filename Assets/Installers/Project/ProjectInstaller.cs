using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.BindInterfacesAndSelfTo<ProjectSetup>().AsSingle().NonLazy();
    }
}