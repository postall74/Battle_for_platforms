using UnityEngine;

/// <summary>
/// Состояние преследования игрока врагом.
/// Враг движется к игроку, пока видит его.
/// Если игрок пропадает из видимости, враг переходит в состояние возврата.
/// </summary>
public class EnemyChaseState : EnemyBaseState
{
    /// <summary>
    /// Конструктор состояния преследования.
    /// </summary>
    /// <param name="context">Контекст состояния врага.</param>
    public EnemyChaseState(EnemyStateContext context)
        : base(context) { }

    /// <summary>
    /// Вход в состояние преследования.
    /// Начинает движение к игроку если он найден.
    /// </summary>
    public override void Enter()
    {
        if (Context.Player != null)
            UpdateMovement();
    }

    /// <summary>
    /// Обновление состояния преследования.
    /// Проверяет видимость игрока и обновляет движение.
    /// </summary>
    /// <param name="deltaTime">Время прошедшее с последнего кадра.</param>
    public override void Update(float deltaTime)
    {
        // Если игрок потерян или не виден, переходим в состояние возврата
        if (Context.Player == null || IsPlayerVisible() == false)
        {
            StateChanger.ChangeState<EnemyReturnState>();
            return;
        }

        UpdateMovement();
    }

    /// <summary>
    /// Выход из состояния преследования.
    /// Останавливает движение врага.
    /// </summary>
    public override void Exit()
    {
        Context.Movement.Stop();
    }

    /// <summary>
    /// Обновление движения врага в сторону игрока.
    /// </summary>
    private void UpdateMovement()
    {
        if (Context.Player == null)
            return;

        float direction = Mathf.Sign(Context.Player.position.x - Context.Transform.position.x);
        Context.Movement.Move(direction);
    }
}