using UnityEngine;
using Zenject;

namespace Installers.Project
{
    [CreateAssetMenu(fileName = "ProjectConfigInstaller", menuName = "Installers/ProjectConfigInstaller")]
    public class ProjectConfigInstaller : ScriptableObjectInstaller<ProjectConfigInstaller>
    {
        [SerializeField] private ProjectSettings projectSettings;
        public override void InstallBindings()
        {
            Container.BindInstances(projectSettings);
        }
    }
}