namespace BattleForPlatforms.Interfaces
{
    /// <summary>
    /// Интерфейс для объекта, способного атаковать прыжком сверху.
    /// </summary>
    public interface IJumpAttackable
    {
        /// <summary>
        /// Попытка атаки прыжком сверху.
        /// </summary>
        /// <param name="attacker">Атакующий объект.</param>
        /// <returns>True, если атака успешна, иначе False.</returns>
        bool TryJumpAttack(IJumpAttackable attacker);

        /// <summary>
        /// Количество урона, получаемого при атаке прыжком.
        /// </summary>
        float JumpAttackDamage { get; }
    }
}
