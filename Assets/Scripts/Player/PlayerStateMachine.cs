using UnityEngine;

/// <summary>
/// Машина состояний игрока.
/// Управляет переключением между различными состояниями игрока (перемещение, прыжок, атака, получение урона, смерть).
/// </summary>
public class PlayerStateMachine : StateMachine
{
    /// <summary>
    /// Конструктор машины состояний игрока.
    /// </summary>
    public PlayerStateMachine() : base() { }
}
