using UnityEngine;

/// <summary>
/// Компонент для нанесения урона при контакте с игроком.
/// Используется на врагах для атаки игрока при столкновении.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ContactDamage : MonoBehaviour
{
    [Header("Настройки урона")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackCooldown = 1f;

    private float _lastAttackTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other.gameObject);
    }

    /// <summary>
    /// Обработка столкновения с объектом.
    /// Если объект имеет HealthProvider, наносит ему урон.
    /// </summary>
    /// <param name="target">Целевой объект.</param>
    private void HandleCollision(GameObject target)
    {
        if (Time.time - _lastAttackTime < _attackCooldown)
            return;

        if (target.TryGetComponent<HealthProvider>(out var healthProvider))
        {
            healthProvider.TakeDamage(_damage);
            _lastAttackTime = Time.time;
        }
    }
}
