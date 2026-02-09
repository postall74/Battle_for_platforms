using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float _attackCooldown = 1f;
    private float _lastAttackTime;
    private bool _isAttacking;

    public EnemyAttackState(EnemyStateContext context) : base(context)
    { }

    public override void Enter()
    {
        _lastAttackTime = -_attackCooldown;
        _isAttacking = false;
    }

    public override void Update(float deltaTime)
    {
        if(Context.Player == null || IsPlayerVisible() == false)
        {
            StateChanger.ChangeState<EnemyChaseState>();
            return;
        }

        if (IsPlayerInAttackRange() == false)
        {
            StateChanger.ChangeState<EnemyChaseState>();
            return;
        }

        Context.Movement.Stop();

        if (Time.time >= _lastAttackTime + _attackCooldown)
        {
            Attack();
            _lastAttackTime = Time.time;
            _isAttacking = true;
        }
    }

    public override void Exit()
    {
        Context.Movement.Stop();
    }

    private bool IsPlayerInAttackRange()
    {
        if (Context.Player == null)
            return false;

        float distance = Vector2.Distance(Context.Transform.position, Context.Player.position);
        return distance <= Context.Attacker.AttackRange;
    }

    private void Attack()
    {
        if (Context.Player.TryGetComponent<IDamageable>(out var damageable))
            Context.Attacker.Attack(damageable);
    }
}