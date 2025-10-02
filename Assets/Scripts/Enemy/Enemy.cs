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

    private EnemyStateMachine _stateMachine;
    private EnemyMovement _movement;
    private EnemyAnimation _animation;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private bool _isDead = false;

    public event Action<int> DamageTaken;
    public event Action Died;
    public event Action<bool> StunnedStateChanged;

    public int Damage => _damage;
    public int Health => _health;
    public bool IsDead => _isDead;

    private void Awake()
    {
        InitializeComponents();
        SubscribeToEvents();
    }


    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    public void TakeDamage(int damage)
    {
        if (_isDead)
            return;

        _health -= damage;
        DamageTaken?.Invoke(damage);

        if (_health <= 0)
            Die();
        else
            StartCoroutine(StunRoutine());
    }

    public void Die()
    {
        _isDead = true;

        Died?.Invoke();
        _stateMachine.SetDead(true);

        StartCoroutine(DieRoutine());
    }

    private void InitializeComponents()
    {
        _stateMachine = GetComponent<EnemyStateMachine>();
        _movement = GetComponent<EnemyMovement>();
        _animation = GetComponent<EnemyAnimation>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    private void SubscribeToEvents()
    {
        _stateMachine.StateChanged += HandleStateChanged;
        _stateMachine.PlayerDetected += HandlePlayerDetected;

        _movement.Movement += _animation.HandleMovement;
    }

    private void UnsubscribeFromEvents()
    {
        if (_stateMachine != null)
        {
            _stateMachine.StateChanged -= HandleStateChanged;
            _stateMachine.PlayerDetected -= HandlePlayerDetected;
        }

        if (_movement != null)
        {
            _movement.Movement -= _animation.HandleMovement;
        }
    }

    private void HandleStateChanged(EnemyStateType newState)
    {
        switch (newState)
        {
            case EnemyStateType.Patrolling:
            case EnemyStateType.Chasing:
            case EnemyStateType.Returning:
                _animation.HandleMovement(_movement.Speed);
                break;
            case EnemyStateType.Attacking:
                _animation.HandleMovement(0);
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

    private IEnumerator StunRoutine()
    {
        StunnedStateChanged?.Invoke(true);
        _stateMachine.SetStunned(true);

        yield return StartCoroutine(FlashColorRoutine(Color.red, _stunDuration));

        _stateMachine.SetStunned(false);
        StunnedStateChanged?.Invoke(false);
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
}