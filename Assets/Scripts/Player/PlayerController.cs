using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(Collector))]
[RequireComponent(typeof(HealthCollector))]
[RequireComponent(typeof(Flipper))]
[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(SpriteBlinker))]
[RequireComponent(typeof(PlayerAttack))]
public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private CharacterMovement _movement;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Collector _collector;
    [SerializeField] private HealthCollector _healthCollector;
    [SerializeField] private Flipper _flipper;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private SpriteBlinker _spriteBlinker;
    [SerializeField] private PlayerAttack _playerAttack;

    private bool _isDead = false;

    public HealthComponent Health => _healthComponent;
    public CharacterMovement Movement => _movement;
    public InputReader InputReader => _inputReader;
    public PlayerAttack PlayerAttack => _playerAttack;

    private void Awake()
    {
        InitializeComponents();
        SubscribeToEvents();
    }

    private void Update()
    {
        if (_isDead) 
            return;

        HandleInput();
        HandleAttack();
    }

    private void FixedUpdate()
    {
        if (_isDead) 
            return;

        HandleMovement();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void InitializeComponents()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _movement = GetComponent<CharacterMovement>();
        _animator = GetComponent<PlayerAnimator>();
        _inputReader = GetComponent<InputReader>();
        _collector = GetComponent<Collector>();
        _healthCollector = GetComponent<HealthCollector>();
        _flipper = GetComponent<Flipper>();
        _groundChecker = GetComponent<GroundChecker>();
        _spriteBlinker = GetComponent<SpriteBlinker>();
        _playerAttack = GetComponent<PlayerAttack>();

        _healthCollector.Initialize(_healthComponent);
    }

    private void SubscribeToEvents()
    {
        if (_groundChecker != null)
            _groundChecker.GroundedChanged += OnGroundedChanged;

        if (_movement != null)
        {
            _movement.Movement += OnMovement;
            _movement.Jumped += OnJumped;
        }

        if (_healthComponent != null)
        {
            _healthComponent.DamageTaken += OnDamageTaken;
            _healthComponent.Healed += OnHealed;
            _healthComponent.Died += OnDied;
        }
    }

    private void UnsubscribeFromEvents()
    {
        if (_groundChecker != null)
            _groundChecker.GroundedChanged -= OnGroundedChanged;

        if (_movement != null)
        {
            _movement.Movement -= OnMovement;
            _movement.Jumped -= OnJumped;
        }

        if (_healthComponent != null)
        {
            _healthComponent.DamageTaken -= OnDamageTaken;
            _healthComponent.Healed -= OnHealed;
            _healthComponent.Died -= OnDied;
        }
    }

    private void OnGroundedChanged(bool isGrounded)
    {
        _animator?.HandleGroundedChanged(isGrounded);
    }

    private void OnMovement(float direction)
    {
        _animator?.HandleMovement(direction);
    }

    private void OnJumped()
    {
        _animator?.HandleJump();
    }

    private void OnDamageTaken(int damage)
    {
        // Обработка уже происходит в HealthComponent через SpriteBlinker
    }

    private void OnHealed(int amount)
    {
        // Обработка уже происходит в HealthComponent через SpriteBlinker
    }

    private void HandleInput()
    {
        if (_inputReader.WasJumpPressed)
            _movement.Jump();
    }

    private void HandleAttack()
    {
        // Атака обрабатывается в PlayerAttack через клавишу E
    }

    private void HandleMovement()
    {
        _movement.Move(_inputReader.HorizontalDirection);
        _animator.HandleVerticalVelocity(_movement.GetVerticalVelocity());
    }

    private void OnDied()
    {
        _isDead = true;
        // Отключаем управление
        enabled = false;

        // Отключаем атаку
        if (_playerAttack != null)
            _playerAttack.enabled = false;

        //Запускаем анимацию
        _animator.HandleDeath();

        //Отключаем движение
        _movement.enabled = false;

        //Отключаем коллайдеры
        var collider = GetComponent<Collider2D>();

        if (collider != null)
            collider.enabled = false;

        Debug.Log("Player died! Game over or Respawn...");

        // Для респавна можно добавить:
        // Invoke(nameof(Respawn), 3f);
    }

    private void Respawn()
    {
        _isDead = false;
        _healthComponent.Revive();
        _animator.HandleRespawn();
        enabled = true;
        _movement.enabled = true;

        if (_playerAttack != null)
            _playerAttack.enabled = true;

        var collider = GetComponent<Collider2D>();

        if (collider != null)
            collider.enabled = true;
    }
}