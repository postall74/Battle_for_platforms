namespace BattleForPlatforms.Interfaces
{
    /// <summary>
    /// Интерфейс для управления анимацией персонажа.
    /// Предоставляет методы для установки параметров анимации.
    /// </summary>
    public interface IAnimated
    {
        /// <summary>
        /// Установка скорости движения для анимации.
        /// </summary>
        /// <param name="speed">Скорость движения.</param>
        void SetMoveSpeed(float speed);

        /// <summary>
        /// Установка состояния нахождения в воздухе.
        /// </summary>
        /// <param name="isInAir">True, если персонаж в воздухе, иначе False.</param>
        void SetInAir(bool isInAir);

        /// <summary>
        /// Установка состояния получения урона.
        /// </summary>
        /// <param name="isHit">True, если персонаж получил урон, иначе False.</param>
        void SetHit(bool isHit);

        /// <summary>
        /// Установка состояния смерти.
        /// </summary>
        /// <param name="isDead">True, если персонаж мертв, иначе False.</param>
        void SetDead(bool isDead);
    }
}
