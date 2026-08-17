namespace BattleForPlatforms.Interfaces
{
    /// <summary>
    /// Интерфейс для объекта, имеющего здоровье.
    /// Предоставляет методы для получения урона и лечения.
    /// </summary>
    public interface IHealth
    {
        /// <summary>
        /// Текущее количество здоровья.
        /// </summary>
        float CurrentHealth { get; }

        /// <summary>
        /// Максимальное количество здоровья.
        /// </summary>
        float MaxHealth { get; }

        /// <summary>
        /// Флаг, указывающий, жив ли объект.
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// Нанесение урона объекту.
        /// </summary>
        /// <param name="damage">Количество урона.</param>
        void TakeDamage(float damage);

        /// <summary>
        /// Лечение объекта.
        /// </summary>
        /// <param name="amount">Количество здоровья для восстановления.</param>
        void Heal(float amount);
    }
}
