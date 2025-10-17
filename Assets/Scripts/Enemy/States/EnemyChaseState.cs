using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateContext context, StateMachine stateMachine)
        : base(context, stateMachine) { }

    public override void Enter()
    {
        if (Context.Player != null)
            UpdateMovement();
    }

    public override void Update()
    {
        if (Context.Player == null || !IsPlayerVisible())
        {
            StateMachine.ChangeState<EnemyReturnState>();
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
        if (Context.Player == null) return;

        float direction = Mathf.Sign(Context.Player.position.x - Context.Transform.position.x);
        Context.Movement.Move(direction);
    }
}