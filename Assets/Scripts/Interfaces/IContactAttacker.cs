namespace BattleForPlatforms.Interfaces
{
    /// <summary>
    /// Интерфейс для объекта, наносящего урон при контакте.
    /// </summary>
    public interface IContactAttacker
    {
        /// <summary>
        /// Количество урона, наносимого при контакте.
        /// </summary>
        float Damage { get; }

        /// <summary>
        /// Атака цели при контакте.
        /// </summary>
        /// <param name="target">Цель атаки.</param>
        void Attack(IHealth target);
    }
}
