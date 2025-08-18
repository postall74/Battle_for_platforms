using UnityEngine;

public class IdleState : State
{
    public IdleState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void Enter() =>
        entity.Animation.PlayIdle();

    public override void UpdateState()
    {
        if (Mathf.Abs(entity.Input.MoveDirection) > 0.1f)
            stateMachine.ChangeState(new RunState(entity, stateMachine));

        if (entity.Input.JumpTriggered && entity.IsGrounded)
            stateMachine.ChangeState(new JumpState(entity, stateMachine));
    }
}