using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateContext context)
        : base(context) { }

    public override void Enter()
    {
        if (Context.Player != null)
            UpdateMovement();
    }

    public override void Update(float deltaTime)
    {
        if (Context.Player != null || IsPlayerVisible() == false)
        {
            StateChanger.ChangeState<EnemyReturnState>();
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
        if (Context.Player == null)
            return;

        float direction = Mathf.Sign(Context.Player.position.x - Context.Transform.position.x);
        Context.Movement.Move(direction);
    }
}