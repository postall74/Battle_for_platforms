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
            UpdateMovement();
    }

    public override void Update()
    {
        if (_player == null)
            return;

        UpdateMovement();
    }

    public override void Exit()
    {
        Movement.Stop();
    }

    private void UpdateMovement()
    {
        float direction = Mathf.Sign(_player.position.x - Transform.position.x);
        Movement.Move(direction);
        Movement.Flip(direction);
    }
}