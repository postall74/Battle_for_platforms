using BattleForPlatforms.Interfaces.States;

namespace BattleForPlatforms.Player.States
{
    /// <summary>
    /// Базовое состояние игрока.
    /// Содержит общую логику для всех состояний игрока.
    /// </summary>
    public abstract class PlayerBaseState : IEnterableState, IUpdatableState, IFixedUpdatableState
    {
        protected readonly PlayerContext Context;

        protected PlayerBaseState(PlayerContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Метод вызывается при входе в состояние.
        /// </summary>
        public virtual void Enter()
        {
        }

        /// <summary>
        /// Метод вызывается при выходе из состояния.
        /// </summary>
        public virtual void Exit()
        {
        }

        /// <summary>
        /// Метод вызывается каждый кадр для обновления логики состояния.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Метод вызывается в фиксированный шаг времени для обновления логики состояния.
        /// </summary>
        public virtual void FixedUpdate()
        {
        }

        /// <summary>
        /// Переключение на состояние движения.
        /// </summary>
        protected void ChangeToMoveState()
        {
            Context.StateMachine.ChangeState<PlayerMoveState>();
        }

        /// <summary>
        /// Переключение на состояние прыжка.
        /// </summary>
        protected void ChangeToJumpState()
        {
            Context.StateMachine.ChangeState<PlayerJumpState>();
        }

        /// <summary>
        /// Переключение на состояние получения урона.
        /// </summary>
        protected void ChangeToDamageState()
        {
            Context.StateMachine.ChangeState<PlayerDamageState>();
        }

        /// <summary>
        /// Переключение на состояние смерти.
        /// </summary>
        protected void ChangeToDeathState()
        {
            Context.StateMachine.ChangeState<PlayerDeathState>();
        }
    }
}
