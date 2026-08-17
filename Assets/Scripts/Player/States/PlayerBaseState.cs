using UnityEngine;

/// <summary>
/// Базовый класс для состояний игрока.
/// Предоставляет общую функциональность для всех состояний игрока.
/// </summary>
public abstract class PlayerBaseState : IEnterableState, IUpdatableState, IFixedUpdatableState
{
    /// <summary>
    /// Контекст состояния игрока, содержащий ссылки на все необходимые компоненты.
    /// </summary>
    protected PlayerContext Context { get; }
    
    /// <summary>
    /// Интерфейс для переключения состояний.
    /// </summary>
    protected IStateChanger StateChanger { get; private set; }

    /// <summary>
    /// Конструктор базового состояния игрока.
    /// </summary>
    /// <param name="context">Контекст состояния игрока.</param>
    protected PlayerBaseState(PlayerContext context)
    {
        Context = context;
    }

    /// <summary>
    /// Устанавливает машину состояний для данного состояния.
    /// </summary>
    /// <param name="stateChanger">Интерфейс для переключения состояний.</param>
    public void SetStateMachine(IStateChanger stateChanger)
    {
        StateChanger = stateChanger;
    }

    /// <summary>
    /// Метод вызывается при входе в состояние.
    /// Должен быть реализован в наследниках.
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// Метод вызывается при выходе из состояния.
    /// Должен быть реализован в наследниках.
    /// </summary>
    public abstract void Exit();

    /// <summary>
    /// Обновление состояния каждый кадр.
    /// </summary>
    /// <param name="deltaTime">Время прошедшее с последнего кадра.</param>
    public abstract void Update(float deltaTime);

    /// <summary>
    /// Физическое обновление состояния.
    /// По умолчанию ничего не делает.
    /// </summary>
    /// <param name="deltaTime">Время прошедшее с последнего физического обновления.</param>
    public virtual void FixedUpdate(float deltaTime) { }

    /// <summary>
    /// Проверяет, находится ли игрок на земле.
    /// </summary>
    /// <returns>True если игрок на земле, иначе false.</returns>
    protected bool IsGrounded()
    {
        return Context.GroundChecker != null && Context.GroundChecker.IsGrounded;
    }
}
