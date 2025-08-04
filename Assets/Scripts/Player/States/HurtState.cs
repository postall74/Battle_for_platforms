using UnityEngine;
using static PlayerAnimationController;

public class HurtState : PlayerState
{
    private float _hurtDuration = 0.8f;
    private float _hurtTimer;

    public HurtState(PlayerController player, PlayerStateMachine stateMachine)
        : base(player, stateMachine) { }

    public override void Enter()
    {
        animator.PlayAnimation(AnimationType.Hurt);
        _hurtTimer = _hurtDuration;
        player.Rigidbody.linearVelocity = new Vector2(-player.transform.localScale.x * 3, 5f);
    }

    public override void LogicUpdate()
    {
        _hurtTimer -= Time.deltaTime;

        if (_hurtTimer <= 0)
        {
            // Возврат в нормальное состояние после получения урона
            stateMachine.ChangeState(player.IsGrounded
                ? new IdleState(player, stateMachine)
                : new FallState(player, stateMachine));
        }
    }
}