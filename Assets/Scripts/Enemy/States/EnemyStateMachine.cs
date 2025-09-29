using System;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private const float PlayerForgetTime = 2f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform _leftPatrolPoint;
    [SerializeField] private Transform _rightPatrolPoint;
    [SerializeField] private float _returnThreshold = 0.5f;

    [Header("Detection Settings")]
    [SerializeField] private float _visionRange = 5f;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private LayerMask _playerLayer;

    private EnemyMovement _movement;
    private Transform _player;
    private Vector2 _startPosition;
    private bool _isFacingRight = false;
    private bool _isStunned = false;
    private bool _isDead = false;

    private bool _playerWasDetected = false;
    private float _lastPlayerDetectionTime = 0f;

    private EnemyState _currentState;
    private EnemyStates _currentStateType = EnemyStates.Patrolling;

    public event Action<EnemyStates> OnStateChanged;
    public event Action<bool> OnPlayerDetected;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
        _startPosition = transform.position;

    }

    private void Start()
    {
        ChangeState(EnemyStates.Patrolling);
    }

    private void Update()
    {
        if (_isDead || _isStunned)
            return;

        UpdatePlayerDerection();

        _currentState?.Update();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        if (_leftPatrolPoint != null && _rightPatrolPoint != null)
            Gizmos.DrawLine(_leftPatrolPoint.position, _rightPatrolPoint.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

    private void UpdatePlayerDerection()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, _visionRange, _playerLayer);
        bool isPlayerCurrentlyDetected = playerCollider != null;

        if (isPlayerCurrentlyDetected)
        {
            _lastPlayerDetectionTime = Time.time;
            _player = playerCollider.transform;
        }
        else if(Time.time - _lastPlayerDetectionTime > PlayerForgetTime)
        {
            _player = null;
        }

        if(isPlayerCurrentlyDetected != _playerWasDetected)
        {
            _playerWasDetected = isPlayerCurrentlyDetected;
            OnPlayerDetected?.Invoke(isPlayerCurrentlyDetected);
        }

        DetermineNewStateBasedOnPlayer();
    }

    private void DetermineNewStateBasedOnPlayer()
    {
        if (_player == null)
        {
            if (_currentStateType != EnemyStates.Patrolling && _currentStateType != EnemyStates.Returning)
                ChangeState(EnemyStates.Returning);

            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        EnemyStates newState = _currentStateType;

        if (distanceToPlayer <= _attackRange)
            newState = EnemyStates.Attacking;
        else if (distanceToPlayer <= _visionRange)
            newState = EnemyStates.Chasing;
        else
            newState = EnemyStates.Returning;

        if(newState != _currentStateType)
            ChangeState(newState);
    }

    private void ChangeState(EnemyStates newState)
    {
        _currentState?.Exit();

        _currentState = CreateState(newState);
        _currentStateType = newState;

        if (_currentState != null)
        {
            _currentState.OnStateCompleted += HandleStateCompleted;
            _currentState.Enter();
        }

        OnStateChanged?.Invoke(newState);
    }

    private EnemyState CreateState(EnemyStates state)
    {
        return state switch
        {
            EnemyStates.Patrolling => new PatrolState(this, _movement, transform, _leftPatrolPoint, _rightPatrolPoint, _isFacingRight),
            EnemyStates.Chasing => new ChaseState(this, _movement, transform, _player),
            EnemyStates.Attacking => new AttackState(this, _movement, transform, _player, _attackRange),
            EnemyStates.Returning => new ReturnState(this, _movement, transform, _startPosition, _returnThreshold),
            _ => new PatrolState(this, _movement, transform, _leftPatrolPoint, _rightPatrolPoint, _isFacingRight)
        };
    }

    private void HandleStateCompleted(EnemyStates nextState)
    {
        ChangeState(nextState);
    }

    public void SetStunned(bool stunned)
    {
        _isStunned = stunned;

        if (stunned)
        {
            _movement.Stop();
            _currentState?.Exit();
        }
        else if(_isDead == false)
        {
            ChangeState(EnemyStates.Patrolling);
        }
    }

    public void SetDead(bool dead)
    {
        _isDead = dead;

        if (dead)
        {
            _movement.Stop();
            _currentState?.Exit();
            _currentState = null;
        }
    }
}