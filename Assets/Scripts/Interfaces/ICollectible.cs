namespace BattleForPlatforms.Interfaces
{
    /// <summary>
    /// Интерфейс для объекта, способного собирать предметы.
    /// </summary>
    public interface ICollector
    {
        /// <summary>
        /// Сбор предмета.
        /// </summary>
        /// <param name="collectible">Собираемый предмет.</param>
        void Collect(ICollectible collectible);
    }

    /// <summary>
    /// Интерфейс собираемого предмета.
    /// </summary>
    public interface ICollectible
    {
        /// <summary>
        /// Эффект от сбора предмета.
        /// </summary>
        /// <param name="collector">Объект, собирающий предмет.</param>
        void Collect(ICollector collector);
    }
}
