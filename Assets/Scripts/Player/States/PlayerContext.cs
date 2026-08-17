using UnityEngine;

/// <summary>
/// Контекст состояния игрока.
/// Содержит ссылки на все компоненты, необходимые для работы состояний игрока.
/// </summary>
public class PlayerContext
{
    /// <summary>
    /// Трансформ игрока.
    /// </summary>
    public Transform Transform { get; }
    
    /// <summary>
    /// Компонент движения игрока.
    /// </summary>
    public CharacterMovement Movement { get; }
    
    /// <summary>
    /// Компонент анимации игрока.
    /// </summary>
    public PlayerAnimator Animator { get; }
    
    /// <summary>
    /// Компонент чтения ввода.
    /// </summary>
    public InputReader InputReader { get; }
    
    /// <summary>
    /// Компонент проверки земли.
    /// </summary>
    public GroundChecker GroundChecker { get; }
    
    /// <summary>
    /// Компонент переворота спрайта.
    /// </summary>
    public Flipper Flipper { get; }
    
    /// <summary>
    /// Компонент здоровья игрока.
    /// </summary>
    public HealthProvider HealthProvider { get; }
    
    /// <summary>
    /// Конструктор контекста состояния игрока.
    /// </summary>
    /// <param name="transform">Трансформ игрока.</param>
    /// <param name="movement">Компонент движения.</param>
    /// <param name="animator">Компонент анимации.</param>
    /// <param name="inputReader">Компонент чтения ввода.</param>
    /// <param name="groundChecker">Компонент проверки земли.</param>
    /// <param name="flipper">Компонент переворота спрайта.</param>
    /// <param name="healthProvider">Компонент здоровья.</param>
    public PlayerContext(Transform transform, CharacterMovement movement, PlayerAnimator animator, 
                         InputReader inputReader, GroundChecker groundChecker, Flipper flipper, HealthProvider healthProvider)
    {
        Transform = transform;
        Movement = movement;
        Animator = animator;
        InputReader = inputReader;
        GroundChecker = groundChecker;
        Flipper = flipper;
        HealthProvider = healthProvider;
    }
}
