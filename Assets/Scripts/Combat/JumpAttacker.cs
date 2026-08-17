using UnityEngine;

namespace BattleForPlatforms.Combat
{
    /// <summary>
    /// Компонент атаки прыжком сверху.
    /// Проверяет столкновение с врагом сверху и наносит урон при успешной атаке.
    /// </summary>
    public class JumpAttacker : MonoBehaviour, IJumpAttackable
    {
        [Header("Настройки атаки")]
        [SerializeField] private float _jumpAttackDamage = 20f;
        [SerializeField] private float _jumpBounceForce = 5f;
        [SerializeField] private float _attackAngleThreshold = 45f;

        /// <summary>
        /// Количество урона при атаке прыжком.
        /// </summary>
        public float JumpAttackDamage => _jumpAttackDamage;

        /// <summary>
        /// Событие, вызываемое при успешной атаке прыжком.
        /// </summary>
        public event System.Action<IJumpAttackable> OnJumpAttack;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var jumpAttackable = collision.gameObject.GetComponent<IJumpAttackable>();
            
            if (jumpAttackable == null) return;

            // Проверка угла атаки (должно быть сверху)
            if (IsAttackFromTop(collision))
            {
                if (TryJumpAttack(jumpAttackable))
                {
                    // Отскок после атаки
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, _jumpBounceForce);
                    }
                }
            }
        }

        /// <summary>
        /// Попытка атаки прыжком сверху.
        /// </summary>
        /// <param name="attacker">Атакующий объект.</param>
        /// <returns>True, если атака успешна, иначе False.</returns>
        public bool TryJumpAttack(IJumpAttackable attacker)
        {
            // В данной реализации мы сами являемся атакующим
            // Этот метод может быть расширен для других случаев
            return true;
        }

        /// <summary>
        /// Проверка, является ли столкновение атакой сверху.
        /// </summary>
        /// <param name="collision">Данные столкновения.</param>
        /// <returns>True, если атака сверху, иначе False.</returns>
        private bool IsAttackFromTop(Collision2D collision)
        {
            // Получаем точку контакта
            if (collision.contactCount == 0) return false;

            ContactPoint2D contact = collision.contacts[0];
            
            // Вычисляем угол между нормалью контакта и вертикалью
            float angle = Vector2.Angle(contact.normal, Vector2.down);
            
            return angle <= _attackAngleThreshold;
        }

        /// <summary>
        /// Нанесение урона цели.
        /// </summary>
        /// <param name="target">Цель атаки.</param>
        public void DealDamage(IHealth target)
        {
            target?.TakeDamage(_jumpAttackDamage);
            OnJumpAttack?.Invoke(this);
        }
    }
}
