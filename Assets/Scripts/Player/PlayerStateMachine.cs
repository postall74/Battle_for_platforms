using System.Collections.Generic;
using BattleForPlatforms.Interfaces.States;

namespace BattleForPlatforms.Player
{
    /// <summary>
    /// Машина состояний игрока.
    /// Управляет переключением между состояниями игрока.
    /// </summary>
    public class PlayerStateMachine : StateMachine.StateMachine
    {
        /// <summary>
        /// Конструктор машины состояний игрока.
        /// </summary>
        /// <param name="context">Контекст игрока со всеми зависимостями.</param>
        public PlayerStateMachine(States.PlayerContext context) 
            : base(CreateStates(context))
        {
        }

        /// <summary>
        /// Создание всех состояний игрока.
        /// </summary>
        /// <param name="context">Контекст игрока.</param>
        /// <returns>Словарь состояний.</returns>
        private static Dictionary<System.Type, IExitableState> CreateStates(States.PlayerContext context)
        {
            return new Dictionary<System.Type, IExitableState>
            {
                { typeof(States.PlayerMoveState), new States.PlayerMoveState(context) },
                { typeof(States.PlayerJumpState), new States.PlayerJumpState(context) },
                { typeof(States.PlayerDamageState), new States.PlayerDamageState(context) },
                { typeof(States.PlayerDeathState), new States.PlayerDeathState(context) }
            };
        }
    }
}
