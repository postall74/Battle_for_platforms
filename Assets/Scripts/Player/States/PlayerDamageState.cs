using UnityEngine;

/// <summary>
/// Состояние получения урона игроком.
/// Обрабатывает момент получения урона и временную неуязвимость.
/// </summary>
public class PlayerDamageState : PlayerBaseState
{
    /// <summary>
    /// Время нахождения в состоянии получения урона.
    /// </summary>
    private const float DamageStateDuration = 0.5f;
    
    /// <summary>
    /// Таймер состояния.
    /// </summary>
    private float _timer;

    /// <summary>
    /// Конструктор состояния получения урона.
    /// </summary>
    /// <param name="context">Контекст состояния игрока.</param>
    public PlayerDamageState(PlayerContext context)
        : base(context) { }

    /// <summary>
    /// Вход в состояние получения урона.
    /// Запускает таймер.
    /// </summary>
    public override void Enter()
    {
        _timer = DamageStateDuration;
        Context.Movement.Stop();
        
        // Проигрываем анимацию получения урона если она есть
        Context.Animator.HandleMovement(0);
    }

    /// <summary>
    /// Обновление состояния получения урона.
    /// Отсчитывает время и переключает состояние обратно на перемещение.
    /// </summary>
    /// <param name="deltaTime">Время прошедшее с последнего кадра.</param>
    public override void Update(float deltaTime)
    {
        _timer -= deltaTime;
        
        if (_timer <= 0f)
        {
            // Проверка на смерть
            if (Context.HealthProvider != null && Context.HealthProvider.IsAlive == false)
            {
                StateChanger.ChangeState<PlayerDeathState>();
            }
            else
            {
                StateChanger.ChangeState<PlayerMoveState>();
            }
            return;
        }
    }

    /// <summary>
    /// Выход из состояния получения урона.
    /// </summary>
    public override void Exit()
    {
        // Ничего не требуется при выходе
    }
}
