using System;
using System.Collections.Generic;

namespace GameEvents
{
    public class GameEventsDispatcher : IGameEventsDispatcher
    {
        private bool _disposed;
        private Dictionary<Type, Delegate> _gameEventHandlers = new ();

        private readonly Queue<Action> _invokeOnUpdate = new ();

        ~GameEventsDispatcher()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            RemoveAllListeners();
            _gameEventHandlers = null;
            GC.SuppressFinalize(this);
        }

        public void AddListener<TEvent>(GameEventHandler<TEvent> handler) where TEvent : IGameEvent
        {
            if (_gameEventHandlers.TryGetValue(typeof(TEvent), out var @delegate))
            {
                _gameEventHandlers[typeof(TEvent)] = Delegate.Combine(@delegate, handler);
            }
            else
            {
                _gameEventHandlers[typeof(TEvent)] = handler;
            }
        }

        public void RemoveListener<TEvent>(GameEventHandler<TEvent> handler) where TEvent : IGameEvent
        {
            if (_gameEventHandlers == null)
            {
                return;
            }

            if (!_gameEventHandlers.TryGetValue(typeof(TEvent), out var @delegate))
            {
                return;
            }

            var current = Delegate.Remove(@delegate, handler);
            if (current == null)
            {
                _gameEventHandlers.Remove(typeof(TEvent));
            }
            else
            {
                _gameEventHandlers[typeof(TEvent)] = current;
            }
        }

        public void Dispatch<TEvent>(TEvent @event) where TEvent : IGameEvent
        {
            if (_gameEventHandlers == null)
            {
                return;
            }

            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (!_gameEventHandlers.TryGetValue(typeof(TEvent), out var @delegate))
            {
                return;
            }
            if (@delegate is GameEventHandler<TEvent> callback)
            {
                callback(@event);
            }
        }

        public void DispatchOnUpdate<TEvent>(TEvent @event) where TEvent : IGameEvent
        {
            _invokeOnUpdate.Enqueue(() => Dispatch(@event));
        }

        public void InvokeEventInQueue()
        {
            var eventAction = _invokeOnUpdate.Dequeue();
            eventAction.Invoke();
        }

        public bool HasEventInQueue()
        {
            return _invokeOnUpdate.Count > 0;
        }

        private void RemoveAllListeners()
        {
            var handlerTypes = new Type[_gameEventHandlers.Keys.Count];
            _gameEventHandlers.Keys.CopyTo(handlerTypes, 0);

            foreach (var handlerType in handlerTypes)
            {
                var delegates = _gameEventHandlers[handlerType].GetInvocationList();
                foreach (var delegate1 in delegates)
                {
                    var handlerToRemove = Delegate.Remove(_gameEventHandlers[handlerType], delegate1);
                    if (handlerToRemove == null)
                    {
                        _gameEventHandlers.Remove(handlerType);
                    }
                    else
                    {
                        _gameEventHandlers[handlerType] = handlerToRemove;
                    }
                }
            }
        }
    }
}
