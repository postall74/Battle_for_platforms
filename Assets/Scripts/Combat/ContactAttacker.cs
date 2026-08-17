using UnityEngine;

/// <summary>
/// Компонент для нанесения урона при контакте.
/// Используется врагами для атаки игрока при столкновении.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ContactAttacker : MonoBehaviour, IContactAttacker
{
    [Header("Настройки атаки")]
    [SerializeField] private int _damage = 10;

    /// <summary>
    /// Количество урона, которое наносит объект.
    /// </summary>
    public int Damage => _damage;

    /// <summary>
    /// Нанести урон при контакте с целью.
    /// </summary>
    /// <param name="target">Цель атаки.</param>
    public void AttackOnContact(IHealth target)
    {
        if (target == null || !target.IsAlive)
            return;

        target.TakeDamage(_damage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IHealth>(out var health))
        {
            AttackOnContact(health);
        }
    }
}
