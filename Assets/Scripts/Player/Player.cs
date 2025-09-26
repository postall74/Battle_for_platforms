using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation), typeof(PlayerMovement))]
public class Player : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private int _health = 1;

    [Header("Collision Settings")]
    [SerializeField] private float _deathForceImpulse = 3f;

    private bool _isDead = false;

    public event Action<int> OnDamageTaken;
    public event Action OnDied;

    public Rigidbody2D Rigidbody { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public InputReader InputReader { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerAnimation Animation { get; private set; }


    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InputReader = GetComponent<InputReader>();
        Movement = GetComponent<PlayerMovement>();
        Animation = GetComponent<PlayerAnimation>();

        Movement.OnGroundedChanged += Animation.HandleGroundedChanged;
        Movement.OnMovement += Animation.HandleMovement;
        Movement.OnJumped += Animation.HandleJump;
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
        {
            foreach (ContactPoint2D contact in collision.contacts)
                TakeDamage(enemy.Damage);
        }
    }

    private void OnDestroy()
    {
        if (Movement != null)
        {
            Movement.OnGroundedChanged -= Animation.HandleGroundedChanged;
            Movement.OnMovement -= Animation.HandleMovement;
            Movement.OnJumped -= Animation.HandleJump;
        }
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