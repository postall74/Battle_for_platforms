using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyStateMachine), typeof(EnemyMovement), typeof(EnemyAnimation))]
public partial class Enemy : MonoBehaviour, IDamageable
{
    [Header("Health settings")]
    [SerializeField] private int _health = 1;

    [Header("Attack Settings")]
    [SerializeField] private int _damage = 1;

    [Header("Stune Settings")]
    [SerializeField] private float _stunDuration = 1f;
    [SerializeField] private float _flashSpeed = 10f;

    [Header("Death Settings")]
    [SerializeField] private float _deathDuration = 3f;
    [SerializeField] private float _deathJumpForce = 5f;

    public event Action<int> OnDamageTaken;
    public event Action OnDied;
    public event Action<bool> OnStunnedStateChanged;

    private EnemyStateMachine _stateMachine;
    private EnemyMovement _movement;
    private EnemyAnimation _animation;
    private SpriteRenderer _spriteRenderer;

    private Color _originalColor;
    private bool _isDead = false;

    public int Damage => _damage;
    public int Health => _health;
    public bool IsDead => _isDead;

    private void Awake()
    {
        _stateMachine = GetComponent<EnemyStateMachine>();
        _movement = GetComponent<EnemyMovement>();
        _animation = GetComponent<EnemyAnimation>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;

        _stateMachine.OnStateChanged += HandleStateChanged;
        _stateMachine.OnPlayerDetected += HandlePlayerDetected;

        _movement.OnMovement += _animation.HandleMovement;
    }

    private void OnDestroy()
    {
        if (_stateMachine != null)
        {
            _stateMachine.OnStateChanged -= HandleStateChanged;
            _stateMachine.OnPlayerDetected -= HandlePlayerDetected;
        }

        if (_movement != null)
        {
            _movement.OnMovement -= _animation.HandleMovement;
        }
    }

    private void HandleStateChanged(EnemyStates newState)
    {
        switch (newState)
        {
            case EnemyStates.Patrolling:
            case EnemyStates.Chasing:
                _animation.HandleMovement(_movement.Speed);
                break;
            case EnemyStates.Attacking:
                _animation.HandleMovement(0);
                break;
            case EnemyStates.Returning:
                _animation.HandleMovement(_movement.Speed);
                break;

        }
    }

    private void HandlePlayerDetected(bool isPlayerDetected)
    {
        if(isPlayerDetected)
        {
#if UNITY_EDITOR
            Debug.Log("Enemy detected player!");
#endif
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Enemy lost player!");
#endif
        }
    }

    private IEnumerator FlashColorRoutine(Color targetColor, float duration, bool resetColor = true)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            _spriteRenderer.color = Color.Lerp(_originalColor, targetColor, Mathf.PingPong(elapsedTime * _flashSpeed, 1));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (resetColor)
            _spriteRenderer.color = _originalColor;
    }

    private IEnumerator StunRoutine()
    {
        OnStunnedStateChanged?.Invoke(true);
        _stateMachine.SetStunned(true);

        yield return StartCoroutine(FlashColorRoutine(Color.red, _stunDuration));

        _stateMachine.SetStunned(false);
        OnStunnedStateChanged?.Invoke(false);
    }

    private IEnumerator DieRoutine()
    {
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null)
            collider.enabled = false;

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        if (rigidbody != null)
            rigidbody.linearVelocity = new Vector2(0, _deathJumpForce);

        yield return StartCoroutine(FlashColorRoutine(Color.red, _deathDuration, false));

        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (_isDead)
            return;

        _health -= damage;
        OnDamageTaken?.Invoke(damage);

        if (_health <= 0)
            Die();
        else
            StartCoroutine(StunRoutine());
    }

    public void Die()
    {
        _isDead = true;

        OnDied?.Invoke();
        _stateMachine.SetDead(true);

        StartCoroutine(DieRoutine());
    }
}