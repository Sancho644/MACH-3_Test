namespace Game.States
{
    public interface IPayLoadState<TPayload> : IExitableState
    {
        public void Enter(TPayload payload);
    }
}