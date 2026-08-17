using UnityEngine;

/// <summary>
/// Состояние перемещения игрока.
/// Обрабатывает ввод и перемещение игрока по горизонтали.
/// </summary>
public class PlayerMoveState : PlayerBaseState
{
    /// <summary>
    /// Конструктор состояния перемещения.
    /// </summary>
    /// <param name="context">Контекст состояния игрока.</param>
    public PlayerMoveState(PlayerContext context)
        : base(context) { }

    /// <summary>
    /// Вход в состояние перемещения.
    /// </summary>
    public override void Enter()
    {
        // Ничего не требуется при входе
    }

    /// <summary>
    /// Обновление состояния перемещения.
    /// Проверяет ввод для прыжка и переключает состояния при необходимости.
    /// </summary>
    /// <param name="deltaTime">Время прошедшее с последнего кадра.</param>
    public override void Update(float deltaTime)
    {
        // Проверка на прыжок
        if (Context.InputReader.WasJumpPressed && IsGrounded())
        {
            StateChanger.ChangeState<PlayerJumpState>();
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
    /// Физическое обновление состояния перемещения.
    /// Обрабатывает перемещение игрока.
    /// </summary>
    /// <param name="deltaTime">Время прошедшее с последнего физического обновления.</param>
    public override void FixedUpdate(float deltaTime)
    {
        float direction = Context.InputReader.HorizontalDirection;
        Context.Movement.Move(direction);
        
        // Обновление анимации вертикальной скорости
        Context.Animator.HandleVerticalVelocity(Context.Movement.GetVerticalVelocity());
    }

    /// <summary>
    /// Выход из состояния перемещения.
    /// Останавливает движение.
    /// </summary>
    public override void Exit()
    {
        Context.Movement.Stop();
    }
}
