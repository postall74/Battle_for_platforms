using System;
using UnityEngine;

/// <summary>
/// Основной компонент игрока.
/// Инициализирует все компоненты, подписывается на события и управляет вводом и движением.
/// </summary>
[RequireComponent(typeof(PlayerAnimator), typeof(CharacterMovement), typeof(Collector), typeof(HealthProvider))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// Rigidbody2D игрока.
    /// </summary>
    public Rigidbody2D Rigidbody { get; private set; }
    
    /// <summary>
    /// SpriteRenderer игрока.
    /// </summary>
    public SpriteRenderer SpriteRenderer { get; private set; }
    
    /// <summary>
    /// Компонент чтения ввода.
    /// </summary>
    public InputReader InputReader { get; private set; }
    
    /// <summary>
    /// Компонент движения.
    /// </summary>
    public CharacterMovement Movement { get; private set; }
    
    /// <summary>
    /// Компонент анимации.
    /// </summary>
    public PlayerAnimator Animator { get; private set; }
    
    /// <summary>
    /// Компонент сбора предметов.
    /// </summary>
    public Collector Collector { get; private set; }
    
    /// <summary>
    /// Компонент переворота спрайта.
    /// </summary>
    public Flipper Flipper { get; private set; }
    
    /// <summary>
    /// Компонент проверки земли.
    /// </summary>
    public GroundChecker GroundChecker { get; private set; }
    
    /// <summary>
    /// Компонент здоровья.
    /// </summary>
    public HealthProvider HealthProvider { get; private set; }
    
    /// <summary>
    /// Машина состояний игрока.
    /// </summary>
    public PlayerStateMachine StateMachine { get; private set; }
    
    /// <summary>
    /// Контекст состояний игрока.
    /// </summary>
    private PlayerContext _context;

    private void Awake()
    {
        InitializeComponents();
        InitializeStateMachine();
        SubscribeToEvents();
    }

    private void Update()
    {
        StateMachine.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate(Time.fixedDeltaTime);
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    /// <summary>
    /// Инициализирует все необходимые компоненты.
    /// </summary>
    private void InitializeComponents()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InputReader = GetComponent<InputReader>();
        Movement = GetComponent<CharacterMovement>();
        Animator = GetComponent<PlayerAnimator>();
        Collector = GetComponent<Collector>();
        Flipper = GetComponent<Flipper>();
        GroundChecker = GetComponent<GroundChecker>();
        HealthProvider = GetComponent<HealthProvider>();
    }

    /// <summary>
    /// Инициализирует машину состояний игрока.
    /// </summary>
    private void InitializeStateMachine()
    {
        _context = new PlayerContext(
            transform,
            Movement,
            Animator,
            InputReader,
            GroundChecker,
            Flipper,
            HealthProvider
        );

        var states = new Dictionary<Type, IExitableState>
        {
            [typeof(PlayerMoveState)] = new PlayerMoveState(_context),
            [typeof(PlayerJumpState)] = new PlayerJumpState(_context),
            [typeof(PlayerDamageState)] = new PlayerDamageState(_context),
            [typeof(PlayerDeathState)] = new PlayerDeathState(_context)
        };

        StateMachine = new PlayerStateMachine(states);

        foreach (var state in states.Values)
        {
            if (state is PlayerBaseState playerState)
                playerState.SetStateMachine(StateMachine);
        }

        StateMachine.ChangeState<PlayerMoveState>();
    }

    /// <summary>
    /// Подписывается на события компонентов.
    /// </summary>
    private void SubscribeToEvents()
    {
        GroundChecker.GroundedChanged += Animator.HandleGroundedChanged;
        Movement.Movement += Animator.HandleMovement;
        Movement.Jumped += Animator.HandleJump;
        HealthProvider.Died += HandleDeath;
        HealthProvider.HealthChanged += HandleHealthChanged;
    }

    /// <summary>
    /// Отписывается от событий компонентов.
    /// </summary>
    private void UnsubscribeFromEvents()
    {
        if (GroundChecker != null)
            GroundChecker.GroundedChanged -= Animator.HandleGroundedChanged;

        if (Movement != null)
        {
            Movement.Movement -= Animator.HandleMovement;
            Movement.Jumped -= Animator.HandleJump;
        }
        
        if (HealthProvider != null)
        {
            HealthProvider.Died -= HandleDeath;
            HealthProvider.HealthChanged -= HandleHealthChanged;
        }
    }

    /// <summary>
    /// Обработчик события смерти.
    /// </summary>
    private void HandleDeath()
    {
        // Здесь можно добавить логику смерти (например, отключение управления)
    }

    /// <summary>
    /// Обработчик изменения здоровья.
    /// Переключает состояние на получение урона если здоровье уменьшилось.
    /// </summary>
    /// <param name="currentHealth">Текущее здоровье.</param>
    /// <param name="maxHealth">Максимальное здоровье.</param>
    private void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        // Если здоровье уменьшилось и игрок еще жив, переключаемся на состояние получения урона
        // Логика обработки урона уже есть в HealthProvider
    }
}