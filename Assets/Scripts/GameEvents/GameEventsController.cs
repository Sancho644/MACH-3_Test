using Core.Services;
using UnityEngine;

namespace GameEvents
{
    public class GameEventsController : MonoBehaviour
    {
        private static GameEventsController _instance;

        private IGameEventsDispatcher _gameEventsDispatcher;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            _gameEventsDispatcher = AllServices.Get<IGameEventsDispatcher>();
        }

        private void Update()
        {
            while (_gameEventsDispatcher.HasEventInQueue())
            {
                _gameEventsDispatcher.InvokeEventInQueue();
            }
        }
    }
}
