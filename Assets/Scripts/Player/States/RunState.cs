using UnityEngine;

public class RunState : State
{
    public RunState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void Enter()
    {
        entity.Animation.PlayRun();
    }

    public override void UpdateState()
    {
        // Проверка перехода в Idle
        if (Mathf.Abs(entity.Input.MoveDirection) < 0.1f)
            stateMachine.ChangeState(new IdleState(entity, stateMachine));

        // Проверка перехода в Jump
        if (entity.Input.JumpTriggered && entity.IsGrounded)
            stateMachine.ChangeState(new JumpState(entity, stateMachine));

        // Проверка перехода в Crouch
        if (entity.Input.IsCrouching && entity.IsGrounded)
            stateMachine.ChangeState(new CrouchState(entity, stateMachine));

        // Проверка перехода в Climb
        if (entity.IsTouchingLadder && Mathf.Abs(entity.Input.VerticalDirection) > 0.1f)
            stateMachine.ChangeState(new ClimbState(entity, stateMachine));
    }

    public override void FixedUpdateState()
    {
        entity.Movement.Move(entity.Input.MoveDirection);
    }
}