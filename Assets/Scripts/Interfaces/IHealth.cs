/// <summary>
/// Интерфейс для объектов, имеющих здоровье.
/// Предоставляет методы для получения урона, лечения и проверки состояния жизни.
/// </summary>
public interface IHealth
{
    /// <summary>
    /// Текущее количество здоровья.
    /// </summary>
    int CurrentHealth { get; }

    /// <summary>
    /// Максимальное количество здоровья.
    /// </summary>
    int MaxHealth { get; }

    /// <summary>
    /// Проверка, жив ли объект.
    /// </summary>
    bool IsAlive { get; }

    /// <summary>
    /// Нанести урон объекту.
    /// </summary>
    /// <param name="damage">Количество урона.</param>
    void TakeDamage(int damage);

    /// <summary>
    /// Восстановить здоровье объекту.
    /// </summary>
    /// <param name="amount">Количество восстанавливаемого здоровья.</param>
    void Heal(int amount);
}
