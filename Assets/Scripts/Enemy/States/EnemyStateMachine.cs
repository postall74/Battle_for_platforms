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
    private EnemyState _currentState;
    private EnemyStateType _currentStateType = EnemyStateType.Patrolling;
    private Transform _player;
    private Vector2 _startPosition;
    private bool _isFacingRight = false;
    private bool _isStunned = false;
    private bool _isDead = false;
    private bool _playerWasDetected = false;
    private float _lastPlayerDetectionTime = 0f;

    public event Action<EnemyStateType> StateChanged;
    public event Action<bool> PlayerDetected;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
        _startPosition = transform.position;
    }

    private void Start()
    {
        ChangeState(EnemyStateType.Patrolling);
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

    public void SetStunned(bool stunned)
    {
        _isStunned = stunned;

        if (stunned)
        {
            _movement.Stop();
            _currentState?.Exit();
        }
        else if (_isDead == false)
        {
            ChangeState(EnemyStateType.Patrolling);
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
            PlayerDetected?.Invoke(isPlayerCurrentlyDetected);
        }

        DetermineNewStateBasedOnPlayer();
    }

    private void DetermineNewStateBasedOnPlayer()
    {
        if (_player == null)
        {
            if (_currentStateType != EnemyStateType.Patrolling && _currentStateType != EnemyStateType.Returning)
                ChangeState(EnemyStateType.Returning);

            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        EnemyStateType newState = _currentStateType;

        if (distanceToPlayer <= _attackRange)
            newState = EnemyStateType.Attacking;
        else if (distanceToPlayer <= _visionRange)
            newState = EnemyStateType.Chasing;
        else
            newState = EnemyStateType.Returning;

        if(newState != _currentStateType)
            ChangeState(newState);
    }

    private void ChangeState(EnemyStateType newState)
    {
        _currentState?.Exit();

        _currentState = CreateState(newState);
        _currentStateType = newState;

        if (_currentState != null)
        {
            _currentState.StateCompleted += HandleStateCompleted;
            _currentState.Enter();
        }

        StateChanged?.Invoke(newState);
    }

    private EnemyState CreateState(EnemyStateType state)
    {
        return state switch
        {
            EnemyStateType.Patrolling => new PatrolState(this, _movement, transform, _leftPatrolPoint, _rightPatrolPoint, _isFacingRight),
            EnemyStateType.Chasing => new ChaseState(this, _movement, transform, _player),
            EnemyStateType.Attacking => new AttackState(this, _movement, transform, _player, _attackRange),
            EnemyStateType.Returning => new ReturnState(this, _movement, transform, _startPosition, _returnThreshold),
            _ => new PatrolState(this, _movement, transform, _leftPatrolPoint, _rightPatrolPoint, _isFacingRight)
        };
    }

    private void HandleStateCompleted(EnemyStateType nextState)
    {
        ChangeState(nextState);
    }
}