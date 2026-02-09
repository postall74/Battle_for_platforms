using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(Flipper))]
[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(SpriteBlinker))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour, IAttacker
{
    [Header("Combat Settings")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackRange = 1.5f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform _leftPatrolPoint;
    [SerializeField] private Transform _rightPatrolPoint;
    [SerializeField] private float _returnThreshold = 0.5f;

    [Header("Detection Settings")]
    [SerializeField] private float _visionRange = 5f;
    [SerializeField] private bool _startFacingRight = false;
    [SerializeField] private LayerMask _playerLayer;

    [Header("Attack Collider")]
    [SerializeField] private EnemyAttackCollider _attackCollider;

    private HealthComponent _healthComponent;
    private EnemyMovement _movement;
    private EnemyAnimator _animator;
    private Flipper _flipper;
    private GroundChecker _groundChecker;
    private Collider2D _enemyCollider;

    private StateMachine _stateMachine;
    private EnemyStateContext _stateContext;
    private Vector2 _initialPosition;

    public int Damage => _damage;
    public float AttackRange => _attackRange;
    public HealthComponent Health => _healthComponent;

    private void Awake()
    {
        _initialPosition = transform.position;
        InitializeComponents();
        InitializeStateMachine();
        SubscribeToEvents();
    }

    private void Start()
    {
        // Настраиваем коллайдер атаки
        if (_attackCollider != null)
            _attackCollider.SetDamage(_damage);

        // Гарантируем, что State Machine запустится с правильного состояния
        _stateMachine.ChangeState<EnemyPatrolState>();
    }

    private void Update()
    {
        _stateMachine.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate(Time.deltaTime);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_leftPatrolPoint != null && _rightPatrolPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(_leftPatrolPoint.position, _rightPatrolPoint.position);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
#endif

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    public bool CanAttack()
    {
        return _stateContext.Player != null &&
               Vector2.Distance(transform.position, _stateContext.Player.position) <= _attackRange;
    }

    public void Attack(IDamageable target)
    {
        if (target != null && target.IsAlive)
            target.TakeDamage(_damage);
    }

    private void InitializeComponents()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _movement = GetComponent<EnemyMovement>();
        _animator = GetComponent<EnemyAnimator>();
        _flipper = GetComponent<Flipper>();
        _groundChecker = GetComponent<GroundChecker>();
        _enemyCollider = GetComponent<Collider2D>();
    }

    private void InitializeStateMachine()
    {
        _stateContext = new EnemyStateContext(
            transform,
            _movement,
            _animator,
            _flipper,
            _groundChecker,
            this,
            _initialPosition,
            _leftPatrolPoint,
            _rightPatrolPoint,
            _visionRange,
            _returnThreshold,
            _playerLayer);

        var stateMachineFactory = new EnemyStateMachineFactory();
        _stateMachine = stateMachineFactory.Create(_stateContext, _startFacingRight);
    }

    private void SubscribeToEvents()
    {
        _movement.Movement += _animator.HandleMovement;
        _healthComponent.DamageTaken += OnDamageTaken;
        _healthComponent.Died += OnDied;
    }

    private void UnsubscribeFromEvents()
    {
        if (_movement != null)
            _movement.Movement -= _animator.HandleMovement;

        if (_healthComponent != null)
        {
            _healthComponent.DamageTaken -= OnDamageTaken;
            _healthComponent.Died -= OnDied;
        }
    }

    private void OnDamageTaken(int damage)
    {
        if (_healthComponent.IsAlive)
            _stateMachine.ChangeState<EnemyDamageState>();
    }

    private void OnDied()
    {
        // Отключаем State Machine
        enabled = false;

        // Запускаем анимацию смерти
        _animator.HandleDeath();

        // Отключаем движение
        _movement.enabled = false;

        // Отключаем коллайдер
        if (_enemyCollider != null)
            _enemyCollider.enabled = false;

        // Отключаем коллайдер атаки
        if (_attackCollider != null)
            _attackCollider.enabled = false;
    }
}