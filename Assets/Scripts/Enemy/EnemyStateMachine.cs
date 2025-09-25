using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform _leftPatrolPoint;
    [SerializeField] private Transform _rightPatrolPoint;
    [SerializeField] private float _returnThreshold = 0.5f;

    [Header("Detection Settings")]
    [SerializeField] private float _visionRange = 5f;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private LayerMask _playerLayer;

    private EnemyStates _currentState = EnemyStates.Patrolling;
    private EnemyMover _movement;
    private Transform _player;
    private Vector2 _startPosition;
    private bool _isFacingRight = false;
    private bool _isStunned = false;
    private bool _isDead = false;

    private void Awake()
    {
        _movement = GetComponent<EnemyMover>();
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (_isDead || _isStunned) 
            return;

        CheckForPlayer();
        UpdateState();
    }

    /// <summary>
    /// Визуализация зон патрулирования, зрения и атаки
    /// </summary>
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

    private void CheckForPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, _visionRange, _playerLayer);

        if (playerCollider != null)
        {
            _player = playerCollider.transform;
            float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

            if (distanceToPlayer <= _attackRange)
                _currentState = EnemyStates.Attacking;
            else if(distanceToPlayer <= _visionRange && IsPlayerInPatrolRange())
                _currentState = EnemyStates.Chasing;
            else
                _currentState = EnemyStates.Returning;
        }
        else if(_currentState != EnemyStates.Patrolling && _currentState != EnemyStates.Returning)
        {
            _currentState = EnemyStates.Returning;
        }
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case EnemyStates.Patrolling:
                Patrol();
                break;
            case EnemyStates.Chasing:
                Chase();
                break;
            case EnemyStates.Attacking:
                Attack();
                break;
            case EnemyStates.Returning:
                ReturnToStart();
                break;
            default:
                break;
        }
    }

    private void Patrol()
    {
        float direction = _isFacingRight ? 1 : -1;
        _movement.Move(direction);

        if ((_isFacingRight && transform.position.x >= _rightPatrolPoint.position.x) ||
            (!_isFacingRight && transform.position.x <= _leftPatrolPoint.position.x))
        {
            _isFacingRight = !_isFacingRight;
            _movement.Flip(_isFacingRight ? 1 : -1);
        }
    }

    private void Chase()
    {
        if (_player == null)
            return;

        float direction = Mathf.Sign(_player.position.x - transform.position.x);
        _movement.Move(direction);
        _movement.Flip(direction);
    }

    private void Attack()
    {
        _movement.Stop();
        _currentState = EnemyStates.Chasing;
    }

    private void ReturnToStart()
    {
        float direction = Mathf.Sign(_startPosition.x - transform.position.x);
        _movement.Move(direction);
        _movement.Flip(direction);

        if (Vector2.Distance(transform.position, _startPosition) < 0.5f)
            _currentState = EnemyStates.Patrolling;
    }

    private bool IsPlayerInPatrolRange()
    {
        return _player.position.x >= _leftPatrolPoint.position.x &&
               _player.position.x <= _rightPatrolPoint.position.x;
    }

    public void SetStunned(bool isStuned)
    {
        _isStunned = isStuned;

        if (isStuned)
            _movement.Stop();
    }

    public void SetDead(bool isDead)
    {
        _isDead = isDead;

        if(isDead)
            _movement.Stop();
    }
}