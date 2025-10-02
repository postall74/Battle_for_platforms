using UnityEngine;

public class AttackState : EnemyState
{
    private Transform _player;
    private float _attackRange;

    public AttackState(EnemyStateMachine stateMachine, EnemyMovement movement, Transform transform,
                       Transform player, float attackRange) : base(stateMachine, movement, transform)
    {
        _player = player;
        _attackRange = attackRange;
    }

    public override void Enter()
    {
        Movement.Stop();
    }

    public override void Update()
    {
        if (_player == null)
            return;

        float distanceToPlayer = Vector2.Distance(Transform.position, _player.position);

        if (distanceToPlayer > _attackRange)
            CompleteState(EnemyStateType.Chasing);
    }

    public override void Exit()
    {
        Movement.Stop();
    }
}