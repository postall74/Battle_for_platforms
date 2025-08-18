using UnityEngine;

public class Player : Entity
{
    protected override void Awake()
    {
        base.Awake();
        StateMachine.Initialize(new IdleState(this, StateMachine));
    }

    protected override void Update()
    {
        base.Update();
        Animation.UpdateAnimations();

        // Глобальные переходы (работают из любого состояния)
        if (UnityEngine.Input.GetKeyDown(KeyCode.R)) // Пример: перезагрузка
        {
            StateMachine.ChangeState(new DeathState(this, StateMachine));
        }

        // Переход в ClimbState
        if (IsTouchingLadder && Mathf.Abs(Input.VerticalDirection) > 0.1f)
        {
            if (!(StateMachine.CurrentState is ClimbState))
                StateMachine.ChangeState(new ClimbState(this, StateMachine));
        }

        // Переход в CrouchState
        if (Input.IsCrouching && IsGrounded)
        {
            if ((StateMachine.CurrentState is CrouchState) == false)
                StateMachine.ChangeState(new CrouchState(this, StateMachine));
        }
    }
}