using BattleForPlatforms.Interfaces.States;

namespace BattleForPlatforms.Enemy.States
{
    /// <summary>
    /// Базовое состояние врага.
    /// Содержит общую логику для всех состояний врага.
    /// </summary>
    public abstract class EnemyBaseState : IEnterableState, IUpdatableState, IFixedUpdatableState
    {
        protected readonly EnemyContext Context;

        protected EnemyBaseState(EnemyContext context)
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
        /// Переключение на состояние патрулирования.
        /// </summary>
        protected void ChangeToPatrolState()
        {
            Context.StateMachine.ChangeState<EnemyPatrolState>();
        }

        /// <summary>
        /// Переключение на состояние преследования.
        /// </summary>
        protected void ChangeToChaseState()
        {
            Context.StateMachine.ChangeState<EnemyChaseState>();
        }

        /// <summary>
        /// Переключение на состояние возврата.
        /// </summary>
        protected void ChangeToReturnState()
        {
            Context.StateMachine.ChangeState<EnemyReturnState>();
        }
    }
}
