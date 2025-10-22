using UnityEngine;

public class EnemyReturnState : EnemyBaseState
{
    public EnemyReturnState(EnemyStateContext context)
        : base(context) { }

    public override void Enter()
    {
        UpdateMovement();
    }

    public override void Update(float deltaTime)
    {
        if (IsPlayerVisible())
        {
            StateChanger.ChangeState<EnemyChaseState>();
            return;
        }

        float distanceToStart = Vector2.Distance(Context.Transform.position, Context.StartPosition);

        if (distanceToStart < Context.ReturnThreshold)
        {
            StateChanger.ChangeState<EnemyPatrolState>();
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