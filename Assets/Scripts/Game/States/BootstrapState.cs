using Core.Services;
using Data;
using Scenes;
using UI.Popups.MainMenu.Services;
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

            RegisterServices();
        }

        public void Enter()
        {
            Debug.Log("Enter Bootstrap state");
            AllServices.Get<ISceneLoaderService>().Load(BootstrapLoadingScene, EnterLoadProgress);
        }

        public void Exit()
        {
        }

        private void EnterLoadProgress()
        {
            _gameStateMachine.Enter<LoadProgressState>();
        }

        private void RegisterServices()
        {
            AllServices.Register<ISceneLoaderService>(new SceneLoaderService(_coroutineRunner));
            AllServices.Register<IPlayerDataService>(new PlayerDataService());
            AllServices.Register<IMainMenuPopupService>(new MainMenuPopupService(AllServices.Get<ISceneLoaderService>()));
        }
    }
}