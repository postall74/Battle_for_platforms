using UnityEngine;

/// <summary>
/// Состояние прыжка игрока.
/// Обрабатывает нахождение игрока в воздухе.
/// </summary>
public class PlayerJumpState : PlayerBaseState
{
    /// <summary>
    /// Конструктор состояния прыжка.
    /// </summary>
    /// <param name="context">Контекст состояния игрока.</param>
    public PlayerJumpState(PlayerContext context)
        : base(context) { }

    /// <summary>
    /// Вход в состояние прыжка.
    /// Выполняет прыжок.
    /// </summary>
    public override void Enter()
    {
        Context.Movement.Jump();
    }

    /// <summary>
    /// Обновление состояния прыжка.
    /// Проверяет приземление и переключает состояния при необходимости.
    /// </summary>
    /// <param name="deltaTime">Время прошедшее с последнего кадра.</param>
    public override void Update(float deltaTime)
    {
        // Проверка на приземление
        if (IsGrounded())
        {
            StateChanger.ChangeState<PlayerMoveState>();
            return;
        }

        // Проверка на получение урона
        if (Context.HealthProvider != null && Context.HealthProvider.IsAlive == false)
        {
            StateChanger.ChangeState<PlayerDeathState>();
            return;
        }
    }

    /// <summary>
    /// Физическое обновление состояния прыжка.
    /// Обновляет анимацию вертикальной скорости.
    /// </summary>
    /// <param name="deltaTime">Время прошедшее с последнего физического обновления.</param>
    public override void FixedUpdate(float deltaTime)
    {
        Context.Animator.HandleVerticalVelocity(Context.Movement.GetVerticalVelocity());
    }

    /// <summary>
    /// Выход из состояния прыжка.
    /// </summary>
    public override void Exit()
    {
        // Ничего не требуется при выходе
    }
}
