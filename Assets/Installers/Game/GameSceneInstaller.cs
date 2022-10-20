using Game;
using Signals;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Element elementPrefab;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SaveSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BoardController>().AsSingle().NonLazy();
        Container.BindFactory<ElementConfigItem, ElementPosition, Element, Element.Factory>().FromComponentInNewPrefab(elementPrefab);
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        BindSignals();
    }

    private void BindSignals()
    {
        Container.DeclareSignal<OnElementClickSignal>();
        Container.DeclareSignal<OnBoardMatchSignal>();
        Container.DeclareSignal<OnScoreChangedSignal>();
        Container.DeclareSignal<OnRestartSignal>();
        Container.DeclareSignal<CreateGameSignal>();
        //Container.DeclareSignal<OnMenuSignal>();
        //Container.DeclareSignal<OnStartSignal>();
    }
}