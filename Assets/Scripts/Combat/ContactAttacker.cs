using UnityEngine;

namespace BattleForPlatforms.Combat
{
    /// <summary>
    /// Компонент контактной атаки.
    /// Наносит урон при столкновении с целью.
    /// </summary>
    public class ContactAttacker : MonoBehaviour, IContactAttacker
    {
        [Header("Настройки атаки")]
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _attackCooldown = 0.5f;

        private float _lastAttackTime;

        /// <summary>
        /// Количество урона, наносимого при контакте.
        /// </summary>
        public float Damage => _damage;

        /// <summary>
        /// Событие, вызываемое при успешной атаке.
        /// </summary>
        public event System.Action<IHealth> OnAttack;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var health = collision.gameObject.GetComponent<IHealth>();
            
            if (health == null) return;

            Attack(health);
        }

        /// <summary>
        /// Атака цели при контакте.
        /// </summary>
        /// <param name="target">Цель атаки.</param>
        public void Attack(IHealth target)
        {
            if (target == null || !target.IsAlive) return;

            float currentTime = Time.time;
            if (currentTime - _lastAttackTime < _attackCooldown) return;

            _lastAttackTime = currentTime;
            target.TakeDamage(_damage);
            OnAttack?.Invoke(target);
        }
    }
}
