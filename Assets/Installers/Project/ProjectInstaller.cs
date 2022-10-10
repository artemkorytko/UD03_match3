using Zenject;

namespace Installers.Project
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProjectSetup>().AsSingle().NonLazy();
        }
    }
}