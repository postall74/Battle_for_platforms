using UnityEngine;

public class JumpState : State
{
    public JumpState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void Enter()
    {
        entity.Animation.PlayJump();
        entity.Movement.Jump();
    }

    public override void UpdateState()
    {
        // Переход в FallState при начале падения
        if (entity.Rigidbody2D.linearVelocity.y < 0)
            stateMachine.ChangeState(new FallState(entity, stateMachine));

        // Переход в Climb во время прыжка
        if (entity.IsTouchingLadder && Mathf.Abs(entity.Input.VerticalDirection) > 0.1f)
            stateMachine.ChangeState(new ClimbState(entity, stateMachine));
    }
}