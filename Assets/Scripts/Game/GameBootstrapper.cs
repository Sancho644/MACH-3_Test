using Game.States;
using Scenes;
using UnityEngine;

namespace Game
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private static GameBootstrapper _instance;
        private GameStateMachine _gameStateMachine;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            if (_instance != null) return;

            var go = new GameObject("[GameBootstrapper]");
            _instance = go.AddComponent<GameBootstrapper>();
            DontDestroyOnLoad(go);
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            _gameStateMachine = new GameStateMachine(this);
            _gameStateMachine.Enter<BootstrapState>();
        }
    }
}