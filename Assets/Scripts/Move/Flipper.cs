using UnityEngine;

namespace BattleForPlatforms.Move
{
    /// <summary>
    /// Компонент разворота персонажа по направлению движения.
    /// Автоматически поворачивает спрайт в сторону движения.
    /// </summary>
    public class Flipper : MonoBehaviour
    {
        [SerializeField] private Transform _visualTransform;
        [SerializeField] private float _facingDirection = 1f;

        private void Awake()
        {
            if (_visualTransform == null)
                _visualTransform = transform;
        }

        /// <summary>
        /// Разворот персонажа в заданном направлении.
        /// </summary>
        /// <param name="direction">Направление (положительное - вправо, отрицательное - влево).</param>
        public void Flip(float direction)
        {
            if (direction == 0f) return;

            if ((direction > 0f && _facingDirection < 0f) || 
                (direction < 0f && _facingDirection > 0f))
            {
                _facingDirection = direction;
                Vector3 scale = _visualTransform.localScale;
                scale.x *= -1;
                _visualTransform.localScale = scale;
            }
        }

        /// <summary>
        /// Принудительный разворот в указанную сторону.
        /// </summary>
        /// <param name="faceRight">True - вправо, False - влево.</param>
        public void FlipToDirection(bool faceRight)
        {
            float targetDirection = faceRight ? 1f : -1f;
            
            if (_facingDirection != targetDirection)
            {
                _facingDirection = targetDirection;
                Vector3 scale = _visualTransform.localScale;
                scale.x = Mathf.Abs(scale.x) * (faceRight ? 1f : -1f);
                _visualTransform.localScale = scale;
            }
        }
    }
}
