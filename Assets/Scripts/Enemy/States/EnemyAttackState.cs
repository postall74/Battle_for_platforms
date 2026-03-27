using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float _attackCooldown = 1f;
    private float _lastAttackTime;
    private bool _isAttacking;

    public EnemyAttackState(EnemyStateContext context) 
        : base(context) { }

    public override void Enter()
    {
        _lastAttackTime = -_attackCooldown;
        _isAttacking = false;
        Context.Movement.Stop();
    }

    public override void Update(float deltaTime)
    {
        // Проверяем, видим ли игрока и в зоне ли атаки
        if(Context.Player == null)
        {
            StateChanger.ChangeState<EnemyChaseState>();
            return;
        }

        if (!IsPlayerInAttackRange())
        {
            StateChanger.ChangeState<EnemyChaseState>();
            return;
        }

        //Останавливаемся для атаки
        Context.Movement.Stop();

        //Проверяем кулдаун атаки
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