public interface IEnemyStateMachineFactory
{
    public StateMachine Create(EnemyStateContext context, bool startFacingRight);
}