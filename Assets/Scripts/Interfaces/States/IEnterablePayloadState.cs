public interface IEnterablePayloadState<TPayload> : IExitableState
{
    public void Enter(TPayload payload);
}
