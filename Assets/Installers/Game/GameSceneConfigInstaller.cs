using Game;
using UnityEngine;
using Zenject;

namespace Installers.Game
{
    [CreateAssetMenu(fileName = "GameSceneConfigInstaller", menuName = "Installers/GameSceneConfigInstaller")]
    public class GameSceneConfigInstaller : ScriptableObjectInstaller<GameSceneConfigInstaller>
    {
        [SerializeField] private ElementsConfig elementsConfig;
        [SerializeField] private BoardConfig boardConfig;
        public override void InstallBindings()
        {
            Container.BindInstances(elementsConfig,boardConfig);
        }
    }
}