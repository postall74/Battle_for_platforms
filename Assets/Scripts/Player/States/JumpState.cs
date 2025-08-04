using UnityEngine;
using static PlayerAnimationController;

public class JumpState : PlayerState
{
    public JumpState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        animator.PlayAnimation(AnimationType.Jump);
        player.Jump();
    }

    public override void LogicUpdate()
    {
        // Переход в падение
        if (player.Rigidbody.linearVelocity.y <= 0)
        {
            stateMachine.ChangeState(new FallState(player, stateMachine));
            return;
        }

        // Переход в лазание
        if (player.IsTouchingLadder && Mathf.Abs(player.InputDirection.y) > 0.1f)
        {
            stateMachine.ChangeState(new ClimbState(player, stateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        player.Move(0.7f); // Сниженная управляемость в воздухе
    }
}