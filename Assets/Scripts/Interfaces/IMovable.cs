namespace BattleForPlatforms.Interfaces
{
    /// <summary>
    /// Интерфейс для управления движением персонажа.
    /// Предоставляет методы для перемещения и прыжков.
    /// </summary>
    public interface IMovable
    {
        /// <summary>
        /// Перемещение персонажа в заданном направлении.
        /// </summary>
        /// <param name="direction">Направление движения (нормализованный вектор).</param>
        void Move(float direction);

        /// <summary>
        /// Выполнение прыжка.
        /// </summary>
        void Jump();

        /// <summary>
        /// Проверка, находится ли персонаж на земле.
        /// </summary>
        /// <returns>True, если персонаж на земле, иначе False.</returns>
        bool IsGrounded();
    }
}
