using System.Threading.Tasks;
using Core.AssetManagement;
using Core.Services;
using Core.Yandex.Services;
using Data.Services;
using Game.Services;
using GameEvents;
using Scenes;
using StaticData;
using UI.Services.Factory;
using UI.Services.Windows;
using UnityEngine;

namespace Game.States
{
    public class BootstrapState : IState
    {
        private const SceneName BootstrapLoadingScene = SceneName.Initial;

        private readonly GameStateMachine _gameStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;

        public BootstrapState(GameStateMachine gameStateMachine, ICoroutineRunner coroutineRunner)
        {
            _gameStateMachine = gameStateMachine;
            _coroutineRunner = coroutineRunner;

            RegisterCoreServices();
        }

        public void Enter()
        {
            Debug.Log("Enter Bootstrap state");
            EnterAsync();
        }

        public void Exit()
        {
        }

        private void EnterLoadProgress()
        {
            _gameStateMachine.Enter<LoadProgressState>();
        }

        private async void EnterAsync()
        {
            await RegisterStaticData();
            RegisterDependentServices();
            AllServices.Get<ISceneLoaderService>().Load(BootstrapLoadingScene, EnterLoadProgress);
        }

        private void RegisterCoreServices()
        {
            RegisterAssetProvider();

            AllServices.Register<ISceneLoaderService>(new SceneLoaderService(_coroutineRunner));
            AllServices.Register<IGameEventsDispatcher>(new GameEventsDispatcher());
        }

        private void RegisterDependentServices()
        {
            AllServices.Register<ILeaderboardService>(new YandexLeaderboardService(AllServices.Get<IStaticDataService>()));
            AllServices.Register<IPlayerDataService>(new PlayerDataService(AllServices.Get<IStaticDataService>()));
            AllServices.Register<IPlayerStatsService>(new PlayerStatsService(
                AllServices.Get<IPlayerDataService>(),
                AllServices.Get<IGameEventsDispatcher>()));
            AllServices.Register<IUIFactoryService>(new UIFactoryService(
                AllServices.Get<IAssets>(),
                AllServices.Get<IStaticDataService>(),
                AllServices.Get<IGameEventsDispatcher>(),
                AllServices.Get<IPlayerStatsService>(),
                AllServices.Get<ILeaderboardService>()));
            AllServices.Register<IWindowService>(new WindowService(AllServices.Get<IUIFactoryService>()));
            AllServices.Register<IGameActionService>(new GameActionService(
                AllServices.Get<ISceneLoaderService>(),
                AllServices.Get<IWindowService>(),
                AllServices.Get<IUIFactoryService>(),
                AllServices.Get<IGameEventsDispatcher>(),
                AllServices.Get<IPlayerStatsService>()));
        }

        private void RegisterAssetProvider()
        {
            AssetsProvider assetProvider = new AssetsProvider();
            assetProvider.Instantiate();
            AllServices.Register<IAssets>(assetProvider);
        }

        private async Task RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService(AllServices.Get<IAssets>());
            await staticData.Initialize();
            AllServices.Register(staticData);
        }
    }
}