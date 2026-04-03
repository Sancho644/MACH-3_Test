using System.Threading.Tasks;
using Core.Services;
using Data.Services;
using Scenes;
using UI.Services.Factory;
using UI.Services.Windows;
using UnityEngine;

namespace Game.States
{
    public class LoadProgressState : IState
    {
        private const SceneName ProgressLoadingScene = SceneName.MainMenu;

        private IWindowService _windowService;
        private IUIFactoryService _iuiFactoryService;

        public void Enter()
        {
            Debug.Log("Enter LoadProgress state");

            LoadProgressOrInitNew();

            _windowService = AllServices.Get<IWindowService>();
            _iuiFactoryService = AllServices.Get<IUIFactoryService>();
            AllServices.Get<ISceneLoaderService>().Load(ProgressLoadingScene, OnLoadComplete);
        }

        public void Exit()
        {
            Debug.Log("Progress loaded");
        }

        private void LoadProgressOrInitNew()
        {
            var playerDataService = AllServices.Get<IPlayerDataService>();
            playerDataService.LoadDefaultPlayerData();
        }

        private async void OnLoadComplete()
        {
            await CreateGameCanvas();
            _windowService.Open(WindowType.MainMenu);
        }

        private async Task CreateGameCanvas() =>
            await _iuiFactoryService.CreateGameCanvas();
    }
}