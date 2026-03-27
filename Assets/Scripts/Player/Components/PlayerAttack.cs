using UnityEngine;

public class PlayerAttack : MonoBehaviour, IAttacker
{
    [Header("Attack Settings")]
    [SerializeField] private int _damage = 15;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _attackPoint;

    private float _lastAttackTime;
    private bool _canAttack = true;
    private PlayerController _controller;

    public int Damage => _damage;

    public float AttackRange => _attackRange;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanAttack())
            Attack();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
        }
    }
#endif

    public bool CanAttack()
    {
        return _canAttack && Time.time > _lastAttackTime + _attackCooldown;
    }

    public void Attack(IDamageable target = null)
    {
        if (CanAttack() == false)
            return;

        _lastAttackTime = Time.time;

        // Если передали конкретную цель
        if (target != null)
        {
            target.TakeDamage(_damage);
            return;
        }

        //Ищем врагов в зоне атаки
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(_damage);
        }
    }

    public void EnableAttack() => _canAttack = true;
    public void DisableAttack() => _canAttack = false;

}
