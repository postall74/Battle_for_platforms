/// <summary>
/// Интерфейс для объектов, которые могут быть атакованы прыжком сверху.
/// Используется для реализации механики "Mario-style" атаки на врагов.
/// </summary>
public interface IJumpAttackable
{
    /// <summary>
    /// Проверка, может ли объект быть атакован прыжком сверху.
    /// </summary>
    bool CanBeJumpAttacked { get; }

    /// <summary>
    /// Обработка атаки прыжком сверху.
    /// </summary>
    /// <param name="attackerTransform">Трансформ атакующего.</param>
    void OnJumpAttack(UnityEngine.Transform attackerTransform);
}
