using UnityEngine;

public class FallState : State
{
    public FallState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void Enter()
    {
        entity.Animation.PlayFall();
    }

    public override void UpdateState()
    {
        // Переход в Idle/Run при приземлении
        if (entity.IsGrounded)
        {
            if (Mathf.Abs(entity.Input.MoveDirection) > 0.1f)
                stateMachine.ChangeState(new RunState(entity, stateMachine));
            else
                stateMachine.ChangeState(new IdleState(entity, stateMachine));
        }

        // Переход в Climb во время падения
        if (entity.IsTouchingLadder && Mathf.Abs(entity.Input.VerticalDirection) > 0.1f)
            stateMachine.ChangeState(new ClimbState(entity, stateMachine));
    }
}