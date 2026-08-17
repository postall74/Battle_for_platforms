using UnityEngine;

/// <summary>
/// Состояние смерти игрока.
/// Обрабатывает смерть персонажа.
/// </summary>
public class PlayerDeathState : PlayerBaseState
{
    /// <summary>
    /// Конструктор состояния смерти.
    /// </summary>
    /// <param name="context">Контекст состояния игрока.</param>
    public PlayerDeathState(PlayerContext context)
        : base(context) { }

    /// <summary>
    /// Вход в состояние смерти.
    /// Останавливает движение и проигрывает анимацию смерти.
    /// </summary>
    public override void Enter()
    {
        Context.Movement.Stop();
        // Здесь можно добавить вызов анимации смерти если она есть
        // Например: Context.Animator.PlayDeathAnimation();
    }

    /// <summary>
    /// Обновление состояния смерти.
    /// В данном состоянии игрок не обновляется.
    /// </summary>
    /// <param name="deltaTime">Время прошедшее с последнего кадра.</param>
    public override void Update(float deltaTime)
    {
        // Игрок мертв, никаких действий
    }

    /// <summary>
    /// Выход из состояния смерти.
    /// </summary>
    public override void Exit()
    {
        // Ничего не требуется при выходе
    }
}
