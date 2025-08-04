using UnityEngine;
using static PlayerAnimationController;

public class IdleState : PlayerState
{
    public IdleState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        animator.PlayAnimation(AnimationType.Idle);
    }

    public override void LogicUpdate()
    {
        // Переходы из состояния покоя
        if (!player.IsGrounded)
        {
            stateMachine.ChangeState(player.Rigidbody.linearVelocity.y > 0
                ? new JumpState(player, stateMachine)
                : new FallState(player, stateMachine));
            return;
        }

        if (player.InputDirection.x != 0)
        {
            stateMachine.ChangeState(new RunState(player, stateMachine));
            return;
        }

        if (player.InputDirection.y < 0 && player.IsGrounded)
        {
            stateMachine.ChangeState(new CrouchState(player, stateMachine));
            return;
        }

        if (player.IsTouchingLadder && Mathf.Abs(player.InputDirection.y) > 0.1f)
        {
            stateMachine.ChangeState(new ClimbState(player, stateMachine));
        }
    }
}