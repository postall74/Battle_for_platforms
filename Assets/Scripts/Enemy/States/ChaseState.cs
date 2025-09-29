using UnityEngine;

public class ChaseState : EnemyState
{
    private Transform _player;

    public ChaseState(EnemyStateMachine stateMachine, EnemyMovement movement, Transform transform, Transform player): base(stateMachine, movement, transform)
    {
        _player = player;
    }

    public override void Enter()
    {
        if (_player != null)
        {
            float direction = Mathf.Sign(_player.position.x - _transform.position.x);
            _movement.Move(direction);
            _movement.Flip(direction);
        }
    }

    public override void Update()
    {
        if (_player == null)
            return;

        float direction = Mathf.Sign(_player.position.x - _transform.position.x);
        _movement.Move(direction);
        _movement.Flip(direction);
    }

    public override void Exit()
    {
        _movement.Stop();
    }
}