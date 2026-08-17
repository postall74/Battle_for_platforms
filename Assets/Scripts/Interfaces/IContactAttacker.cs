/// <summary>
/// Интерфейс для объектов, которые могут атаковать при контакте.
/// </summary>
public interface IContactAttacker
{
    /// <summary>
    /// Нанести урон при контакте с целью.
    /// </summary>
    /// <param name="target">Цель атаки.</param>
    void AttackOnContact(IHealth target);
}
