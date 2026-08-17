using BattleForPlatforms.Interfaces.States;
using UnityEngine;

namespace BattleForPlatforms.Enemy.States
{
    /// <summary>
    /// Состояние преследования врага.
    /// Враг следует за игроком, пока видит его.
    /// Если игрок выходит из зоны видимости, враг возвращается к патрулированию.
    /// </summary>
    public class EnemyChaseState : EnemyBaseState
    {
        private float _chaseSpeed = 3.5f;

        public EnemyChaseState(EnemyContext context) : base(context)
        {
        }

        /// <summary>
        /// Вход в состояние преследования.
        /// Устанавливает скорость преследования.
        /// </summary>
        public override void Enter()
        {
            Context.Animator?.SetMoveSpeed(_chaseSpeed);
        }

        /// <summary>
        /// Обновление состояния преследования каждый кадр.
        /// Проверяет, не потерял ли враг игрока из виду.
        /// </summary>
        public override void Update()
        {
            CheckForPlayerLoss();
        }

        /// <summary>
        /// Фиксированное обновление состояния преследования.
        /// Выполняет движение к игроку.
        /// </summary>
        public override void FixedUpdate()
        {
            ChasePlayer();
        }

        /// <summary>
        /// Проверка потери видимости игрока.
        /// Если игрок не виден, переключается на возврат.
        /// </summary>
        private void CheckForPlayerLoss()
        {
            if (Context.PlayerTransform == null || Context.Vision == null)
            {
                ChangeToReturnState();
                return;
            }

            if (!Context.Vision.CanSeeTarget(Context.PlayerTransform.position))
            {
                ChangeToReturnState();
            }
        }

        /// <summary>
        /// Преследование игрока.
        /// Движение в направлении игрока.
        /// </summary>
        private void ChasePlayer()
        {
            if (Context.Movable == null || Context.PlayerTransform == null) return;

            Vector2 direction = (Context.PlayerTransform.position - transform.position).normalized;
            float moveDirection = Mathf.Sign(direction.x);

            Context.Movable.Move(moveDirection);

            // Разворот в сторону игрока
            var flipper = (Context.Movable as MonoBehaviour)?.GetComponent<Move.Flipper>();
            flipper?.FlipToDirection(moveDirection > 0);
        }
    }
}
