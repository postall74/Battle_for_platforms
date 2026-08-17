using BattleForPlatforms.Interfaces.States;
using UnityEngine;

namespace BattleForPlatforms.Enemy.States
{
    /// <summary>
    /// Состояние патрулирования врага.
    /// Враг перемещается между точками патрулирования в своей зоне.
    /// </summary>
    public class EnemyPatrolState : EnemyBaseState
    {
        private float _currentPoint;
        private float _patrolSpeed = 2f;
        private bool _movingRight = true;

        public EnemyPatrolState(EnemyContext context) : base(context)
        {
        }

        /// <summary>
        /// Вход в состояние патрулирования.
        /// Инициализирует направление движения.
        /// </summary>
        public override void Enter()
        {
            _currentPoint = Context.PatrolZone?.LeftX ?? transform.position.x - 5f;
            Context.Animator?.SetMoveSpeed(_patrolSpeed);
        }

        /// <summary>
        /// Обновление состояния патрулирования каждый кадр.
        /// Проверяет видимость игрока и перемещается.
        /// </summary>
        public override void Update()
        {
            CheckForPlayer();
        }

        /// <summary>
        /// Фиксированное обновление состояния патрулирования.
        /// Выполняет перемещение между точками.
        /// </summary>
        public override void FixedUpdate()
        {
            Patrol();
        }

        /// <summary>
        /// Проверка видимости игрока.
        /// Если игрок найден, переключается на преследование.
        /// </summary>
        private void CheckForPlayer()
        {
            if (Context.PlayerTransform == null || Context.Vision == null) return;

            if (Context.Vision.CanSeeTarget(Context.PlayerTransform.position))
            {
                ChangeToChaseState();
            }
        }

        /// <summary>
        /// Патрулирование зоны.
        /// Перемещение между левой и правой границами зоны.
        /// </summary>
        private void Patrol()
        {
            if (Context.Movable == null || Context.PatrolZone == null) return;

            float currentX = transform.position.x;
            float targetX = _movingRight ? Context.PatrolZone.RightX : Context.PatrolZone.LeftX;

            // Проверка достижения точки
            if ((_movingRight && currentX >= targetX) || (!_movingRight && currentX <= targetX))
            {
                _movingRight = !_movingRight;
                Flip();
            }

            // Движение к цели
            float direction = _movingRight ? 1f : -1f;
            Context.Movable.Move(direction);
            
            // Разворот
            if (Context.Animator != null)
            {
                var flipper = (Context.Movable as MonoBehaviour)?.GetComponent<Move.Flipper>();
                flipper?.FlipToDirection(_movingRight);
            }
        }

        /// <summary>
        /// Разворот врага.
        /// </summary>
        private void Flip()
        {
            if (Context.Movable is MonoBehaviour mb)
            {
                var flipper = mb.GetComponent<Move.Flipper>();
                flipper?.Flip(_movingRight ? 1f : -1f);
            }
        }
    }
}
