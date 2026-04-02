using System;
using JetBrains.Annotations;

namespace GameEvents
{
    public delegate void GameEventHandler<in TEvent>([NotNull] TEvent @event)
        where TEvent : IGameEvent;

    public interface IGameEventsDispatcher : IDisposable
    {
        void AddListener<TEvent>([NotNull] GameEventHandler<TEvent> handler)
            where TEvent : IGameEvent;
        void RemoveListener<TEvent>([NotNull] GameEventHandler<TEvent> handler)
            where TEvent : IGameEvent;
        void Dispatch<TEvent>([NotNull] TEvent @event) where TEvent : IGameEvent;
        void DispatchOnUpdate<TEvent>([NotNull] TEvent @event) where TEvent : IGameEvent;
        void InvokeEventInQueue();
        bool HasEventInQueue();
    }
}