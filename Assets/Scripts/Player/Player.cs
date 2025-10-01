using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation), typeof(CharacterMovement))]
public class Player : MonoBehaviour
{
    private CollectibleController _collectibleController;
    private int _score = 0;

    public event Action<int> ScoreChanged;

    public Rigidbody2D Rigidbody { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public InputReader InputReader { get; private set; }
    public CharacterMovement Movement { get; private set; }
    public PlayerAnimation Animation { get; private set; }
    public int Score => _score;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InputReader = GetComponent<InputReader>();
        Movement = GetComponent<PlayerMovement>();
        Animation = GetComponent<PlayerAnimation>();

        _collectibleController = FindAnyObjectByType<CollectibleController>();

        if (_collectibleController != null)
            _collectibleController.OnScoreChanged += HandleScoreChanged;

        Movement.GroundedChanged += Animation.HandleGroundedChanged;
        Movement.Movement += Animation.HandleMovement;
        Movement.Jumped += Animation.HandleJump;
    }

    private void Update()
    {
        if (_isDead)
            return;

        if (InputReader.WasKeyJumpPressed())
            Movement.Jump();
    }

    private void FixedUpdate()
    {
        if (_isDead)
            return;

        Movement.Move(InputReader.HorizontalDirection);
        Animation.HandleVerticalVelocity(Movement.GetVerticalVelocity());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isDead)
            return;

        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            TakeDamage(enemy.Damage);
    }

    private void OnDestroy()
    {
        if (_collectibleController != null)
            _collectibleController.OnScoreChanged -= HandleScoreChanged;

        if (Movement != null)
        {
            Movement.GroundedChanged -= Animation.HandleGroundedChanged;
            Movement.Movement -= Animation.HandleMovement;
            Movement.Jumped -= Animation.HandleJump;
        }
    }

    private void HandleScoreChanged(int newScore)
    {
        _score = newScore;
        ScoreChanged?.Invoke(_score);

#if UNITY_EDITOR
        Debug.Log($"Player collected coin! Total score: {_score}");
#endif
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        OnDamageTaken?.Invoke(damage);

        if (_health <= 0)
            Die();
    }

    public void Die()
    {
        _isDead = true;
        OnDied?.Invoke();

        Animation.HandleDeath();
        Rigidbody.AddForce(Vector2.up * _deathForceImpulse, ForceMode2D.Impulse);

        enabled = false;
        Movement.enabled = false;

        Time.timeScale = 0;
    }
}