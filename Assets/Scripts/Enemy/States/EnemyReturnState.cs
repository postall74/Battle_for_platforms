using UnityEngine;

public class EnemyReturnState : EnemyBaseState
{
    public EnemyReturnState(EnemyStateContext context, StateMachine stateMachine)
        : base(context, stateMachine) { }

    public override void Enter()
    {
        UpdateMovement();
    }

    public override void Update()
    {
        if (IsPlayerVisible())
        {
            StateMachine.ChangeState<EnemyChaseState>();
            return;
        }

        float distanceToStart = Vector2.Distance(Context.Transform.position, Context.StartPosition);

        if (distanceToStart < Context.ReturnThreshold)
        {
            StateMachine.ChangeState<EnemyPatrolState>();
            return;
        }

        UpdateMovement();
    }

    public override void Exit()
    {
        Context.Movement.Stop();
    }

    private void UpdateMovement()
    {
        float direction = Mathf.Sign(Context.StartPosition.x - Context.Transform.position.x);
        Context.Movement.Move(direction);
    }
}