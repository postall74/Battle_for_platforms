using UnityEngine;

[RequireComponent(typeof(PlayerAnimation), typeof(PlayerMovement))]
public class Player : MonoBehaviour, IDamageable, ICollectible
{
    [Header("Health Settings")]
    [SerializeField] private int _health = 1;

    [Header("Collision Settings")]
    [SerializeField] private float _deathForceImpulse = 3f;

    private bool _isDead = false;
    private int _currentScore = 0;

    public Rigidbody2D Rigidbody { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public InputReader InputReader { get; private set; }
    public PlayerMovement Mover { get; private set; }
    public PlayerAnimation Animation { get; private set; }
    public int Score => _currentScore;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InputReader = GetComponent<InputReader>();
        Mover = GetComponent<PlayerMovement>();
        Animation = GetComponent<PlayerAnimation>();
    }

    private void FixedUpdate()
    {
        if (_isDead) 
            return;

        Mover.Move(InputReader.HorizontalDirection);
        Animation.PlayAnimationRun(InputReader.HorizontalDirection);

        if (InputReader.WasKeyJumpPressed())
        {
            Mover.Jump();
            Animation.TriggerJump();
        }

        Animation.PlayAnimationJumpOrFall(Mover.GetVerticalVelocity(), Mover.IsGrounded);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Coin>(out Coin coin))
            Collect(coin);
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

    public void Collect(Coin coin)
    {
        _currentScore += coin.ScoreValue;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
            Die();
    }

    public void Die()
    {
        _isDead = true;
        Animation.PlayAnimationDie();

        Rigidbody.AddForce(Vector2.up * _deathForceImpulse, ForceMode2D.Impulse);

        enabled = false;
        Mover.enabled = false;

        Time.timeScale = 0;
    }
}