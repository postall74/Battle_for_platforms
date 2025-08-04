using UnityEngine;
using static PlayerAnimationController;

public class ClimbState : PlayerState
{
    public ClimbState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        animator.PlayAnimation(AnimationType.Climb);
        player.Climb();
    }

    public override void Exit()
    {
        player.StopClimbing();
    }

    public override void LogicUpdate()
    {
        // Выход из лазания
        if (!player.IsTouchingLadder || Mathf.Abs(player.InputDirection.y) < 0.1f)
        {
            stateMachine.ChangeState(player.IsGrounded
                ? new IdleState(player, stateMachine)
                : new FallState(player, stateMachine));
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        player.Climb();
    }
}