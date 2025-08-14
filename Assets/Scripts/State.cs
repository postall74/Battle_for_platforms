public abstract class State
{
    protected Entity entity;
    protected StateMachine stateMachine;

    protected State(Entity entity, StateMachine stateMachine)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void Exit() { }
}