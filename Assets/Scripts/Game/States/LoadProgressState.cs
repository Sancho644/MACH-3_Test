using Core.Services;
using Data;
using UnityEngine;

namespace Game.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public LoadProgressState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();

            //_gameStateMachine.Enter<>();
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