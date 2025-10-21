using UnityEngine;

public abstract class EnemyBaseState : IEnterableState, IUpdatableState
{
    protected EnemyStateContext Context { get; }
    protected IStateChanger StateChanger { get; private set; }

    protected EnemyBaseState(EnemyStateContext context)
    {
        Context = context;
    }

    public void SetStateMachine(IStateChanger stateChanger)
    {
        StateChanger = stateChanger;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update(float deltaTime);

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