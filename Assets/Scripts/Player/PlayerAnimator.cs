using UnityEngine;

namespace BattleForPlatforms.Player
{
    /// <summary>
    /// Компонент анимации игрока.
    /// Управляет параметрами аниматора на основе состояния игрока.
    /// </summary>
    public class PlayerAnimator : MonoBehaviour, IAnimated
    {
        [Header("Компоненты")]
        [SerializeField] private Animator _animator;

        private PlayerAnimatorData _animatorData;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();

            _animatorData = new PlayerAnimatorData();
        }

        /// <summary>
        /// Установка скорости движения для анимации.
        /// </summary>
        /// <param name="speed">Скорость движения.</param>
        public void SetMoveSpeed(float speed)
        {
            if (_animator == null) return;
            _animator.SetFloat(_animatorData.MoveSpeedHash, Mathf.Abs(speed));
        }

        /// <summary>
        /// Установка состояния нахождения в воздухе.
        /// </summary>
        /// <param name="isInAir">True, если персонаж в воздухе, иначе False.</param>
        public void SetInAir(bool isInAir)
        {
            if (_animator == null) return;
            _animator.SetBool(_animatorData.InAirHash, isInAir);
        }

        /// <summary>
        /// Установка состояния получения урона.
        /// </summary>
        /// <param name="isHit">True, если персонаж получил урон, иначе False.</param>
        public void SetHit(bool isHit)
        {
            if (_animator == null) return;
            _animator.SetTrigger(_animatorData.HitHash);
        }

        /// <summary>
        /// Установка состояния смерти.
        /// </summary>
        /// <param name="isDead">True, если персонаж мертв, иначе False.</param>
        public void SetDead(bool isDead)
        {
            if (_animator == null) return;
            _animator.SetBool(_animatorData.DeadHash, isDead);
        }
    }
}
