using UnityEngine;

public class ChaseState : EnemyState
{
    private Transform _player;
    private Flipper _flipper;

    public ChaseState(EnemyStateMachine stateMachine, EnemyMovement movement, Transform transform, 
                      Flipper flipper, Transform player): base(stateMachine, movement, transform)
    {
        _player = player;
        _flipper = flipper;
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
    }
}