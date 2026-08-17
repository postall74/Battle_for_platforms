using UnityEngine;

namespace BattleForPlatforms.Enemy.Providers
{
    /// <summary>
    /// Данные для зоны патрулирования врага.
    /// Определяет границы зоны, в которой враг патрулирует.
    /// </summary>
    public class PatrolZone : MonoBehaviour
    {
        [Header("Границы зоны")]
        [SerializeField] private Transform _leftPoint;
        [SerializeField] private Transform _rightPoint;

        /// <summary>
        /// Левая граница зоны патрулирования.
        /// </summary>
        public float LeftX => _leftPoint != null ? _leftPoint.position.x : transform.position.x - 5f;

        /// <summary>
        /// Правая граница зоны патрулирования.
        /// </summary>
        public float RightX => _rightPoint != null ? _rightPoint.position.x : transform.position.x + 5f;

        /// <summary>
        /// Проверка, находится ли точка в зоне патрулирования.
        /// </summary>
        /// <param name="xPosition">X координата точки.</param>
        /// <returns>True, если точка в зоне, иначе False.</returns>
        public bool IsInZone(float xPosition)
        {
            return xPosition >= LeftX && xPosition <= RightX;
        }

        /// <summary>
        /// Получение случайной точки в зоне патрулирования.
        /// </summary>
        /// <returns>X координата случайной точки.</returns>
        public float GetRandomPoint()
        {
            return Random.Range(LeftX, RightX);
        }

        /// <summary>
        /// Отладочная визуализация зоны патрулирования.
        /// </summary>
        private void OnDrawGizmos()
        {
            float left = LeftX;
            float right = RightX;
            Vector3 center = new Vector3((left + right) / 2f, transform.position.y, 0);
            Vector3 size = new Vector3(right - left, 0.5f, 0);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(center, size);

            if (_leftPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_leftPoint.position, 0.3f);
            }

            if (_rightPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_rightPoint.position, 0.3f);
            }
        }
    }
}
