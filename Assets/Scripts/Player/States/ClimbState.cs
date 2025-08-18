using UnityEngine;

public class ClimbState : State
{
    public ClimbState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void Enter()
    {
        entity.Animation.PlayClimb();
        entity.Movement.Climb(entity.Input.VerticalDirection);
    }

    public override void UpdateState()
    {
        // Выход из лестницы
        if (entity.IsTouchingLadder == false)
        {
            if (entity.IsGrounded)
                stateMachine.ChangeState(new IdleState(entity, stateMachine));
            else
                stateMachine.ChangeState(new FallState(entity, stateMachine));
        }

        // Прыжок с лестницы
        if (entity.Input.JumpTriggered)
            stateMachine.ChangeState(new JumpState(entity, stateMachine));
    }

    public override void FixedUpdateState()
    {
        entity.Movement.Climb(entity.Input.VerticalDirection);
    }

    public override void Exit()
    {
        entity.Movement.ApplyGravity();
    }
}