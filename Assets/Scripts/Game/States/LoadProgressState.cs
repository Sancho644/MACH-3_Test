using Core.Services;
using Data;
using Scenes;
using UnityEngine;

namespace Game.States
{
    public class LoadProgressState : IState
    {
        private const SceneName ProgressLoadingScene = SceneName.MainMenu;

        public void Enter()
        {
            LoadProgressOrInitNew();

            AllServices.Get<ISceneLoaderService>().Load(ProgressLoadingScene, () => {});
        }

        public void Exit()
        {
            Debug.Log("Progress loaded");
        }

        private void LoadProgressOrInitNew()
        {
            var playerDataService = AllServices.Get<IPlayerDataService>();
            if (playerDataService.HasPlayerData)
            {
                playerDataService.LoadPlayerData();
            }
            else
            {
                playerDataService.LoadDefaultPlayerData();
            }
        }
    }
}