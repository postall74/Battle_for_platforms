using System;

namespace BattleForPlatforms.Interfaces.States
{
    /// <summary>
    /// Интерфейс состояния, которое можно покинуть.
    /// Используется в машине состояний для корректного выхода из текущего состояния.
    /// </summary>
    public interface IExitableState
    {
        /// <summary>
        /// Метод вызывается при выходе из состояния.
        /// </summary>
        void Exit();
    }

    /// <summary>
    /// Интерфейс состояния, в которое можно войти.
    /// Используется в машине состояний для инициализации нового состояния.
    /// </summary>
    public interface IEnterableState : IExitableState
    {
        /// <summary>
        /// Метод вызывается при входе в состояние.
        /// </summary>
        void Enter();
    }

    /// <summary>
    /// Интерфейс состояния, которое обновляется каждый кадр.
    /// Используется для логики, требующей частого обновления (например, движение).
    /// </summary>
    public interface IUpdatableState : IEnterableState
    {
        /// <summary>
        /// Метод вызывается каждый кадр для обновления логики состояния.
        /// </summary>
        void Update();
    }

    /// <summary>
    /// Интерфейс состояния, которое обновляется в фиксированный шаг времени.
    /// Используется для физики и другой логики, требующей стабильного шага времени.
    /// </summary>
    public interface IFixedUpdatableState : IEnterableState
    {
        /// <summary>
        /// Метод вызывается в фиксированный шаг времени для обновления логики состояния.
        /// </summary>
        void FixedUpdate();
    }

    /// <summary>
    /// Интерфейс состояния, принимающего параметр при входе.
    /// Используется для передачи данных в состояние при переходе.
    /// </summary>
    /// <typeparam name="T">Тип параметра, передаваемого в состояние.</typeparam>
    public interface IEnterablePayloadState<in T> : IExitableState
    {
        /// <summary>
        /// Метод вызывается при входе в состояние с передачей параметра.
        /// </summary>
        /// <param name="payload">Параметр, передаваемый в состояние.</param>
        void Enter(T payload);
    }
}
