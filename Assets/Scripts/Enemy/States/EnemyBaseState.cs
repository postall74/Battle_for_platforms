using UnityEngine;

public abstract class EnemyBaseState : IState
{
    protected EnemyStateContext Context { get; }
    protected StateMachine StateMachine { get; }

    protected EnemyBaseState(EnemyStateContext context, StateMachine stateMachine)
    {
        Context = context;
        StateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void FixedUpdate() { }
    public virtual void Update() { }

    protected bool IsPlayerVisible()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(
            Context.Transform.position,
            Context.VisionRange,
            Context.PlayerLayer);

        if (playerCollider != null)
        {
            Context.Player = playerCollider.transform;
            return true;
        }

        Context.Player = null;
        return false;
    }
}