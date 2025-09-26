using System;
using System.Collections;
using UnityEngine;

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

    public event Action<int> OnDamageTaken;
    public event Action OnDied;
    public event Action OnStunned;
    public event Action OnStunEnded;

    public int Damage => _damage;
    public int Health => _health;
    public bool IsDead => _isDead;

    private void Awake()
    {
        _stateMachine = GetComponent<EnemyStateMachine>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animation = GetComponent<EnemyAnimation>();
        _movement = GetComponent<EnemyMovement>();
        _originalColor = _spriteRenderer.color;

        _stateMachine.OnStateChanged += HandleStateChanged;
        _stateMachine.OnPlayerDetected += HandlePlayerDetected; //TODO: надо будет сделать метод для разварота enemy в стороне Player и направиться к нему, для атаки
        _stateMachine.OnPlayerLost += HandlePlayerLost;  //TODO: надо будет сделать метод для возврата к патрулированию после потери Player из зоны видимости

        _movement.OnMovement += _animation.HandleMovement;
        _movement.OnGroundedChanged += _animation.HandleGroundedChanged;
    }

    private void OnDestroy()
    {
        if (_stateMachine != null)
        {
            _stateMachine.OnStateChanged -= HandleStateChanged;
            _stateMachine.OnPlayerDetected -= HandlePlayerDetected; //TODO: надо будет сделать метод для разварота enemy в стороне Player и направиться к нему, для атаки
            _stateMachine.OnPlayerLost -= HandlePlayerLost;  //TODO: надо будет сделать метод для возврата к патрулированию после потери Player из зоны видимости
        }

        if (_movement != null)
        {
            _movement.OnMovement -= _animation.HandleMovement;
            _movement.OnGroundedChanged -= _animation.HandleGroundedChanged;
        }
    }

    private void HandleStateChanged(EnemyStates newState)
    {
        _animation.HandleStateChanged(newState);

        switch (newState)
        {
            case EnemyStates.Attacking:
                HandleAttack();
                break;
        }
    }

    private void HandlePlayerDetected(Transform player)
    {

    }

    private void HandlePlayerLost()
    {

    }

    private void HandleAttack()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetColor"></param>
    /// <param name="duration"></param>
    /// <param name="resetColor"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator StunRoutine()
    {
        OnStunned?.Invoke();
        _stateMachine.SetStunned(true);

        yield return StartCoroutine(FlashColorRoutine(Color.red, _stunDuration));

        OnStunEnded?.Invoke();
        _stateMachine.SetStunned(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage"></param>
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

    /// <summary>
    /// 
    /// </summary>
    public void Die()
    {
        _isDead = true;
        OnDied?.Invoke();
        _stateMachine.SetDead(true);
        StartCoroutine(DieRoutine());
    }
}