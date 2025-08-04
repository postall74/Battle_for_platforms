using UnityEngine;
using static PlayerAnimationController;

public class RunState : PlayerState
{
    public RunState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        animator.PlayAnimation(AnimationType.Run);
    }

    public override void LogicUpdate()
    {
        // Обработка остановки
        if (Mathf.Abs(player.InputDirection.x) < 0.1f)
        {
            stateMachine.ChangeState(new IdleState(player, stateMachine));
            return;
        }

        // Обработка прыжка
        if (Input.GetButtonDown("Jump") && player.IsGrounded)
        {
            stateMachine.ChangeState(new JumpState(player, stateMachine));
            return;
        }

        // Обработка падения
        if (player.IsGrounded == false)
            stateMachine.ChangeState(new FallState(player, stateMachine));
    }

    public override void PhysicsUpdate()
    {
        player.Move();
    }
}