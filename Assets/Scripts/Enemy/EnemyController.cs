using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform _leftPatrolPoint;
    [SerializeField] private Transform _rightPatrolPoint;
    [SerializeField] private float _returnThreshold = 0.5f;

    [Header("Detection Settings")]
    [SerializeField] private float _visionRange = 5f;
    [SerializeField] private bool _startFacingRight = false;
    [SerializeField] private LayerMask _playerLayer;

    private EnemyMovement _movement;
    private EnemyAnimator _animator;
    private Flipper _flipper;
    private GroundChecker _groundChecker;

    private StateMachine _stateMachine;
    private EnemyStateContext _stateContext;

    private void Awake()
    {
        InitializeComponents();
        InitializeStateMachine();
        SubscribeToEvents();
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void InitializeComponents()
    {
        _movement = GetComponent<EnemyMovement>();
        _animator = GetComponent<EnemyAnimator>();
        _flipper = GetComponent<Flipper>();
        _groundChecker = GetComponent<GroundChecker>();
    }

    private void InitializeStateMachine()
    {
        _stateContext = new EnemyStateContext(
            transform,
            _movement,
            _animator,
            _flipper,
            _groundChecker,
            transform.position,
            _leftPatrolPoint,
            _rightPatrolPoint,
            _visionRange,
            _returnThreshold,
            _playerLayer);

        _stateMachine = new StateMachine();
        _stateMachine.AddState(new EnemyPatrolState(_stateContext, _stateMachine, _startFacingRight));
        _stateMachine.AddState(new EnemyChaseState(_stateContext, _stateMachine));
        _stateMachine.AddState(new EnemyReturnState(_stateContext, _stateMachine));
        _stateMachine.StateChanged += OnStateChanged;

        _stateMachine.ChangeState<EnemyPatrolState>();
    }

    private void SubscribeToEvents()
    {
        _movement.Movement += _animator.HandleMovement;
    }

    private void UnsubscribeFromEvents()
    {
        if (_movement != null)
            _movement.Movement -= _animator.HandleMovement;

        if (_stateMachine != null)
            _stateMachine.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(IState newState)
    {
#if UNITY_EDITOR
        Debug.Log($"Enemy changed state to: {newState.GetType().Name}");
#endif
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
    }
#endif
}