using UnityEngine;

/// <summary>
/// Компонент здоровья и получения урона для врага.
/// Реализует интерфейс IJumpAttackable для поддержки атаки прыжком сверху.
/// </summary>
[RequireComponent(typeof(HealthProvider))]
public class EnemyHealth : MonoBehaviour, IJumpAttackable
{
    [Header("Настройки урона от прыжка")]
    [SerializeField] private int _jumpAttackDamage = 100;

    private HealthProvider _healthProvider;

    /// <summary>
    /// Проверка, может ли враг быть атакован прыжком сверху.
    /// Враг может быть атакован только если он жив.
    /// </summary>
    public bool CanBeJumpAttacked => _healthProvider != null && _healthProvider.IsAlive;

    private void Awake()
    {
        _healthProvider = GetComponent<HealthProvider>();
    }

    /// <summary>
    /// Обработка атаки прыжком сверху.
    /// Наносит урон врагу при успешной атаке.
    /// </summary>
    /// <param name="attackerTransform">Трансформ атакующего (игрока).</param>
    public void OnJumpAttack(Transform attackerTransform)
    {
        if (_healthProvider == null || !_healthProvider.IsAlive)
            return;

        _healthProvider.TakeDamage(_jumpAttackDamage);
    }
}
