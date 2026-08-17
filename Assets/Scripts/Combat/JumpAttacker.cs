using UnityEngine;

/// <summary>
/// Компонент для атаки прыжком сверху.
/// При столкновении с врагом сверху наносит ему урон и отпрыгивает.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class JumpAttacker : MonoBehaviour
{
    [Header("Настройки атаки")]
    [SerializeField] private int _damage = 20;
    [SerializeField] private float _bounceForce = 10f;
    [SerializeField] private float _stompAngle = 135f;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверка, что мы падаем вниз
        if (_rigidbody.linearVelocity.y >= 0)
            return;

        // Проверка угла столкновения (должно быть сверху)
        foreach (var contact in collision.contacts)
        {
            float angle = Vector2.Angle(Vector2.up, contact.normal);
            if (angle < _stompAngle)
            {
                // Попытка атаковать врага
                if (collision.gameObject.TryGetComponent<IJumpAttackable>(out var jumpAttackable))
                {
                    if (jumpAttackable.CanBeJumpAttacked)
                    {
                        jumpAttackable.OnJumpAttack(transform);
                        PerformBounce();
                    }
                }
                break;
            }
        }
    }

    /// <summary>
    /// Выполнить отскок после атаки.
    /// </summary>
    private void PerformBounce()
    {
        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _bounceForce);
    }
}
