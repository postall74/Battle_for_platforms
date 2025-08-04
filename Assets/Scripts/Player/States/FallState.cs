using UnityEngine;
using static PlayerAnimationController;

public class FallState : PlayerState
{
    public FallState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        animator.PlayAnimation(AnimationType.Fall);
    }

    public override void LogicUpdate()
    {
        // Приземление
        if (player.IsGrounded)
        {
            stateMachine.ChangeState(player.InputDirection.x != 0
                ? new RunState(player, stateMachine)
                : new IdleState(player, stateMachine));
            return;
        }

        // Начало лазания
        if (player.IsTouchingLadder && Mathf.Abs(player.InputDirection.y) > 0.1f)
        {
            stateMachine.ChangeState(new ClimbState(player, stateMachine));
            return;
        }

        // Прыжок во время падения (двойной прыжок)
        if (Input.GetButtonDown("Jump"))
        {
            stateMachine.ChangeState(new JumpState(player, stateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        player.Move(0.7f); // Сниженная управляемость в воздухе
    }
}