using System.Collections;
using UnityEngine;

public partial class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health = 1;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _stunDuration = 1f;
    [SerializeField] private float _deathDuration = 3f;
    [SerializeField] private float _deathJumpForce = 5f;

    private EnemyStateMachine _stateMachine;
    private EnemyMover _movement;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private bool _isDead = false;

    public int Damage => _damage;

    private void Awake()
    {
        _stateMachine = GetComponent<EnemyStateMachine>();
        _movement = GetComponent<EnemyMover>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        if(_isDead) 
            return;

        _health -= damage;

        if (_health <= 0)
            Die();
        else
            StartCoroutine(StunRoutine());
    }

    private IEnumerator FlashColorRoutine(Color targetColor, float duration, bool resetColor = true)
    {
        float elapsedTime = 0f;
        float flashSpeed = 10;

        while (elapsedTime < duration)
        {
            _spriteRenderer.color = Color.Lerp(_originalColor, targetColor, Mathf.PingPong(elapsedTime * flashSpeed, 1));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (resetColor)
            _spriteRenderer.color = _originalColor;
    }

    private IEnumerator StunRoutine()
    {
        _stateMachine.SetStunned(true);
        yield return StartCoroutine(FlashColorRoutine(Color.red, _stunDuration));
        _stateMachine.SetStunned(false);
    }

    private IEnumerator DieRoutine()
    {
        Collider2D collider= GetComponent<Collider2D>();

        if (collider != null)
            collider.enabled = false;

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        if (rigidbody != null)
            rigidbody.linearVelocity = new Vector2(0, _deathJumpForce);

        yield return StartCoroutine(FlashColorRoutine(Color.red, _deathDuration, false));

        Destroy(gameObject);
    }

    public void Die()
    {
        _isDead = true;
        _stateMachine.SetDead(true);
        StartCoroutine(DieRoutine());
    }
}
