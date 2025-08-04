public abstract class PlayerState
{
    protected readonly PlayerController player;
    protected readonly PlayerStateMachine stateMachine;
    protected readonly PlayerAnimationController animator;

    protected PlayerState(PlayerController player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        animator = player.Animator;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() { }
}