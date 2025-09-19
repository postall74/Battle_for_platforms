using UnityEngine;

[RequireComponent(typeof(PlayerAnimation), typeof(Mover))]
public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health = 1;

    private int _damageValue = 1;
    private float _dieForeceImpulse = 3f;
    private bool _isDead = false;

    public Rigidbody2D Rigidbody { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public InputReader InputReader { get; private set; }
    public Mover Mover { get; private set; }
    public PlayerAnimation Animation { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InputReader = GetComponent<InputReader>();
        Mover = GetComponent<Mover>();
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

        Animation.PlayAnimationJumpOrFall(Mover.GetVerticalVelocity(), Mover.IsGrounded());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isDead) 
            return;

        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    enemy.TakeDamage(_damageValue);
                    Rigidbody.AddForce(Vector2.up * _dieForeceImpulse, ForceMode2D.Impulse);
                    return;
                }
            }

            TakeDamage(enemy.Damage);
        }
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

        enabled = false;
        Mover.enabled = false;

        Time.timeScale = 0;
    }
}