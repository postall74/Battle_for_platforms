public interface IStateChanger
{
    public void ChangeState<TState>() where TState : class, IEnterableState;
    public void ChangeState<TState, TPayload>(TPayload payload) where TState : class, IEnterablePayloadState<TPayload>;
}
