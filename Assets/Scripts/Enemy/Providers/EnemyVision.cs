using UnityEngine;

namespace BattleForPlatforms.Enemy.Providers
{
    /// <summary>
    /// Провайдер видимости для врага.
    /// Определяет, видит ли враг игрока.
    /// </summary>
    public class EnemyVision : MonoBehaviour
    {
        [Header("Настройки видимости")]
        [SerializeField] private float _viewDistance = 10f;
        [SerializeField] private float _viewAngle = 90f;
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private LayerMask _obstacleLayer;

        /// <summary>
        /// Проверка, видит ли враг цель.
        /// </summary>
        /// <param name="targetPosition">Позиция цели.</param>
        /// <returns>True, если цель видна, иначе False.</returns>
        public bool CanSeeTarget(Vector2 targetPosition)
        {
            Vector2 directionToTarget = targetPosition - (Vector2)transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            // Проверка расстояния
            if (distanceToTarget > _viewDistance)
                return false;

            // Проверка угла обзора
            float angleToTarget = Vector2.Angle(transform.right, directionToTarget);
            if (angleToTarget > _viewAngle / 2f)
                return false;

            // Проверка препятствий (Raycast)
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                directionToTarget.normalized,
                distanceToTarget,
                _obstacleLayer
            );

            if (hit.collider != null)
                return false;

            return true;
        }

        /// <summary>
        /// Отладочная визуализация зоны видимости.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, transform.right * _viewDistance);
            
            Vector3 leftAngle = Quaternion.Euler(0, 0, _viewAngle / 2f) * transform.right;
            Vector3 rightAngle = Quaternion.Euler(0, 0, -_viewAngle / 2f) * transform.right;
            
            Gizmos.DrawRay(transform.position, leftAngle * _viewDistance);
            Gizmos.DrawRay(transform.position, rightAngle * _viewDistance);
        }
    }
}
