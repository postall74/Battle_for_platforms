using UnityEngine;

/// <summary>
/// Компонент управления здоровьем персонажа.
/// Отвечает за получение урона, лечение и смерть.
/// </summary>
public class HealthProvider : MonoBehaviour, IHealth
{
    [Header("Настройки здоровья")]
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _invincibilityDuration = 1f;

    private int _currentHealth;
    private bool _isInvincible;
    private float _invincibilityTimer;

    /// <summary>
    /// Текущее количество здоровья.
    /// </summary>
    public int CurrentHealth => _currentHealth;

    /// <summary>
    /// Максимальное количество здоровья.
    /// </summary>
    public int MaxHealth => _maxHealth;

    /// <summary>
    /// Проверка, жив ли объект.
    /// </summary>
    public bool IsAlive => _currentHealth > 0;

    /// <summary>
    /// Событие изменения здоровья.
    /// </summary>
    public event System.Action<int, int> HealthChanged;

    /// <summary>
    /// Событие смерти.
    /// </summary>
    public event System.Action Died;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _isInvincible = false;
        _invincibilityTimer = 0f;
    }

    private void Update()
    {
        if (_isInvincible)
        {
            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0f)
                _isInvincible = false;
        }
    }

    /// <summary>
    /// Нанести урон объекту.
    /// </summary>
    /// <param name="damage">Количество урона.</param>
    public void TakeDamage(int damage)
    {
        if (!IsAlive || _isInvincible)
            return;

        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (!IsAlive)
        {
            Died?.Invoke();
        }
        else
        {
            _isInvincible = true;
            _invincibilityTimer = _invincibilityDuration;
        }
    }

    /// <summary>
    /// Восстановить здоровье объекту.
    /// </summary>
    /// <param name="amount">Количество восстанавливаемого здоровья.</param>
    public void Heal(int amount)
    {
        if (!IsAlive)
            return;

        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    /// <summary>
    /// Установить значение здоровья (для тестов).
    /// </summary>
    /// <param name="health">Новое значение здоровья.</param>
    public void SetHealth(int health)
    {
        _currentHealth = Mathf.Clamp(health, 0, _maxHealth);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
}
