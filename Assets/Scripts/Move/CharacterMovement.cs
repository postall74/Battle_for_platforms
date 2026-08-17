using UnityEngine;

namespace BattleForPlatforms.Move
{
    /// <summary>
    /// Компонент движения персонажа.
    /// Реализует перемещение и прыжки с использованием физики Unity.
    /// </summary>
    public class CharacterMovement : MonoBehaviour, IMovable
    {
        [Header("Настройки движения")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 10f;

        [Header("Компоненты")]
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private Rigidbody2D _rigidbody;

        private bool _isJumping;

        private void Awake()
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody2D>();
            
            if (_groundChecker == null)
                _groundChecker = GetComponentInChildren<GroundChecker>();
        }

        /// <summary>
        /// Перемещение персонажа в заданном направлении.
        /// </summary>
        /// <param name="direction">Направление движения (-1, 0, 1).</param>
        public void Move(float direction)
        {
            if (_rigidbody == null) return;

            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _moveSpeed;
            _rigidbody.velocity = velocity;
        }

        /// <summary>
        /// Выполнение прыжка.
        /// Прыжок возможен только если персонаж на земле.
        /// </summary>
        public void Jump()
        {
            if (!IsGrounded() || _rigidbody == null) return;

            Vector2 jumpVelocity = Vector2.up * _jumpForce;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpVelocity.y);
            _isJumping = true;
        }

        /// <summary>
        /// Проверка, находится ли персонаж на земле.
        /// </summary>
        /// <returns>True, если персонаж на земле, иначе False.</returns>
        public bool IsGrounded()
        {
            return _groundChecker?.IsGrounded() ?? false;
        }

        /// <summary>
        /// Сброс флага прыжка (вызывается из анимации).
        /// </summary>
        public void ResetJumpFlag()
        {
            _isJumping = false;
        }
    }
}
