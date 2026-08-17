/// <summary>
/// Интерфейс для объектов, способных наносить урон.
/// </summary>
public interface IDamageDealer
{
    /// <summary>
    /// Количество урона, которое наносит объект.
    /// </summary>
    int Damage { get; }

    /// <summary>
    /// Нанести урон цели.
    /// </summary>
    /// <param name="target">Цель, которой наносится урон.</param>
    void DealDamage(IHealth target);
}
