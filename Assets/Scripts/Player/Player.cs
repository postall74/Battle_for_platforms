using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation), typeof(CharacterMovement))]
public class Player : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] private float _deathForceImpulse = 3f;

    public Rigidbody2D Rigidbody { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public InputReader InputReader { get; private set; }
    public CharacterMovement Movement { get; private set; }
    public PlayerAnimation Animation { get; private set; }
    public int Score => _score;
    public int Health => _health;

    public event Action<int> DamageTaken;
    public event Action Died;
    public event Action<int> ScoreChanged;

    private bool _isDead = false;
    private int _score = 0;
    private int _health = 1;

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
    }

    private void FixedUpdate()
    {
        if (_isDead)
            return;

        HandleMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isDead)
            return;

        HandleCollectible(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isDead)
            return;

        HandleEnemyCollision(collision);
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void Collect(ICollectible collectible)
    {
        _score += collectible.ScoreValue;
        ScoreChanged?.Invoke(_score);
        collectible.Collect();

#if UNITY_EDITOR
        Debug.Log($"Player collected item! Total score: {_score}");
#endif
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        DamageTaken?.Invoke(_health);

        if (_health <= 0)
            Die();
    }

    private void InitializeComponents()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InputReader = GetComponent<InputReader>();
        Movement = GetComponent<CharacterMovement>();
        Animation = GetComponent<PlayerAnimation>();
    }

    public void Die()
    {
        _isDead = true;
        Died?.Invoke();

        Animation.HandleDeath();
        Rigidbody.AddForce(Vector2.up * _deathForceImpulse, ForceMode2D.Impulse);

        enabled = false;
        Movement.enabled = false;
        Time.timeScale = 0;
    }

    private void SubscribeToEvents()
    {
        Movement.GroundedChanged += Animation.HandleGroundedChanged;
        Movement.Movement += Animation.HandleMovement;
        Movement.Jumped += Animation.HandleJump;
    }

    private void UnsubscribeFromEvents()
    {
        if (Movement != null)
        {
            Movement.GroundedChanged -= Animation.HandleGroundedChanged;
            Movement.Movement -= Animation.HandleMovement;
            Movement.Jumped -= Animation.HandleJump;
        }
    }

    private void HandleMovement()
    {
        Movement.Move(InputReader.HorizontalDirection);
        Animation.HandleVerticalVelocity(Movement.GetVerticalVelocity());

    }

    private void HandleInput()
    {
        if (InputReader.WasJumpPressed)
            Movement.Jump();
    }

    private void HandleCollectible(Collider2D collision)
    {
        if (collision.TryGetComponent<ICollectible>(out var collectible))
            Collect(collectible);
    }

    private void HandleEnemyCollision(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out var enemy))
            TakeDamage(enemy.Damage);
    }
}