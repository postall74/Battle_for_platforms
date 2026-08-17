using UnityEngine;

namespace BattleForPlatforms.Move
{
    /// <summary>
    /// Компонент проверки нахождения на земле.
    /// Использует射线检测 для определения, стоит ли персонаж на поверхности.
    /// </summary>
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private float _checkRadius = 0.2f;
        [SerializeField] private LayerMask _groundLayer;

        /// <summary>
        /// Проверка, находится ли персонаж на земле.
        /// </summary>
        /// <returns>True, если персонаж на земле, иначе False.</returns>
        public bool IsGrounded()
        {
            if (_groundCheckPoint == null)
            {
                Debug.LogWarning("Ground check point not assigned!");
                return false;
            }

            return Physics2D.OverlapCircle(_groundCheckPoint.position, _checkRadius, _groundLayer);
        }

        /// <summary>
        /// Отладочная визуализация точки проверки земли.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (_groundCheckPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_groundCheckPoint.position, _checkRadius);
            }
        }
    }
}
