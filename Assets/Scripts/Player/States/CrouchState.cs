using UnityEngine;

public class CrouchState : State
{
    public CrouchState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void Enter()
    {
        entity.Animation.PlayCrouch();
        entity.Movement.Crouch();
    }

    public override void UpdateState()
    {
        // ¬ыход из приседани€
        if (!entity.Input.IsCrouching && entity.CanStand)
        {
            if (Mathf.Abs(entity.Input.MoveDirection) > 0.1f)
                stateMachine.ChangeState(new RunState(entity, stateMachine));
            else
                stateMachine.ChangeState(new IdleState(entity, stateMachine));
        }

        // ≈сли над головой по€вилось преп€тствие - остаемс€ в приседе
        if (entity.CanStand == false)
            entity.Animation.PlayCrouch();
    }

    public override void Exit()
    {
        entity.Movement.Stand();
    }
}