using GameEvents;

namespace Core.Match3.GameEvents
{
    public class PauseInputEvent : IGameEvent
    {
        public bool Pause { get; set; }

        public PauseInputEvent(bool pause)
        {
            Pause = pause;
        }
    }
}