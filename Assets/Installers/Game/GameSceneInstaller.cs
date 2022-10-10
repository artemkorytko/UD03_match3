using Game;
using UnityEngine;
using Zenject;

namespace Installers.Game
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private Element elementPrefab;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BoardController>().AsSingle().NonLazy();
            Container.BindFactory<ElementConfigItem, ElementPosition, Element, Element.Factory>()
                .FromComponentInNewPrefab(elementPrefab);
        }
    }
}