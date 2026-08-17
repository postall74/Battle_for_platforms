using UnityEngine;

namespace BattleForPlatforms.Health
{
    /// <summary>
    /// Компонент управления здоровьем сущности.
    /// Реализует получение урона, лечение и смерть.
    /// </summary>
    public class HealthProvider : MonoBehaviour, IHealth
    {
        [Header("Настройки здоровья")]
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _invincibilityDuration = 1f;

        private float _currentHealth;
        private bool _isInvincible;
        private bool _isDead;

        /// <summary>
        /// Событие, вызываемое при изменении здоровья.
        /// </summary>
        public event System.Action<float, float> OnHealthChanged;

        /// <summary>
        /// Событие, вызываемое при смерти.
        /// </summary>
        public event System.Action OnDied;

        /// <summary>
        /// Текущее количество здоровья.
        /// </summary>
        public float CurrentHealth => _currentHealth;

        /// <summary>
        /// Максимальное количество здоровья.
        /// </summary>
        public float MaxHealth => _maxHealth;

        /// <summary>
        /// Флаг, указывающий, жив ли объект.
        /// </summary>
        public bool IsAlive => !_isDead;

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        /// <summary>
        /// Нанесение урона объекту.
        /// </summary>
        /// <param name="damage">Количество урона.</param>
        public void TakeDamage(float damage)
        {
            if (_isDead || _isInvincible) return;

            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                Die();
            }
            else if (_invincibilityDuration > 0)
            {
                StartCoroutine(InvincibilityCoroutine());
            }
        }

        /// <summary>
        /// Лечение объекта.
        /// </summary>
        /// <param name="amount">Количество здоровья для восстановления.</param>
        public void Heal(float amount)
        {
            if (_isDead) return;

            _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        /// <summary>
        /// Смерть объекта.
        /// </summary>
        private void Die()
        {
            _isDead = true;
            OnDied?.Invoke();
        }

        /// <summary>
        /// Корутина временной неуязвимости.
        /// </summary>
        private System.Collections.IEnumerator InvincibilityCoroutine()
        {
            _isInvincible = true;
            yield return new WaitForSeconds(_invincibilityDuration);
            _isInvincible = false;
        }

        /// <summary>
        /// Сброс состояния смерти (для респауна).
        /// </summary>
        public void ResetHealth()
        {
            _isDead = false;
            _currentHealth = _maxHealth;
            _isInvincible = false;
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }
    }
}
