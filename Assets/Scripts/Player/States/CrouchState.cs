using UnityEngine;
using static PlayerAnimationController;

public class CrouchState : PlayerState
{
    private Vector2 _originalColliderSize;
    private Vector2 _originalColliderOffset;
    private Vector2 _crouchColliderSize = new(1f, 1f);
    private Vector2 _crouchColliderOffset = new(0f, -0.5f);

    public CrouchState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        animator.PlayAnimation(AnimationType.Crouch);

        // Сохраняем оригинальные параметры коллайдера
        _originalColliderSize = player.Collider.size;
        _originalColliderOffset = player.Collider.offset;

        // Устанавливаем коллайдер для приседания
        player.Collider.size = _crouchColliderSize;
        player.Collider.offset = _crouchColliderOffset;
    }

    public override void Exit()
    {
        // Восстанавливаем оригинальный коллайдер
        player.Collider.size = _originalColliderSize;
        player.Collider.offset = _originalColliderOffset;
    }

    public override void LogicUpdate()
    {
        // Выход из приседания
        if (player.InputDirection.y >= 0 || !player.IsGrounded)
        {
            stateMachine.ChangeState(player.InputDirection.x != 0
                ? new RunState(player, stateMachine)
                : new IdleState(player, stateMachine));
            return;
        }

        // Переход в лазание
        if (player.IsTouchingLadder && Mathf.Abs(player.InputDirection.y) > 0.1f)
        {
            stateMachine.ChangeState(new ClimbState(player, stateMachine));
        }
    }
}