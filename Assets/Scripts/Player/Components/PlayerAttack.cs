using UnityEngine;

public class PlayerAttack : MonoBehaviour, IAttacker
{
    [Header("Attack Settings")]
    [SerializeField] private int _damage = 1;
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
        if (_controller.InputReader.WasAttackKeyPressed && CanAttack())
        {

        }
    }

    public void Attack(IDamageable target)
    {
        throw new System.NotImplementedException();
    }

    public bool CanAttack()
    {
        throw new System.NotImplementedException();
    }
}
