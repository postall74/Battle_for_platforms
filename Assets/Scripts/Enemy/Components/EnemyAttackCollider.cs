using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyAttackCollider : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private LayerMask _attackableLayers;

    private Collider2D _attackCollider;
    private float _lastAttackTime;
    private bool _canAttack = true;

    private void Awake()
    {
        _attackCollider = GetComponent<Collider2D>();
        _attackCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleAttackCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleAttackCollision(collision);
    }

    private void HandleAttackCollision(Collider2D collision)
    {
        if (_canAttack == false)
            return;

        if (((1 << collision.gameObject.layer) & _attackableLayers) == 0)
            return;

        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            if (damageable.IsAlive && Time.time >= _lastAttackTime + _attackCooldown)
            {
                damageable.TakeDamage(_damage);
                _lastAttackTime = Time.time;
            }
        }
    }

    public void EnableAttack() => _canAttack = true;
    public void DisableAttack() => _canAttack = false;
    public void SetDamage(int damage) => _damage = damage;
}