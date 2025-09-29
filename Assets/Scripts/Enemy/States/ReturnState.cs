using UnityEngine;

public class ReturnState : EnemyState
{
    private readonly Vector2 _startPosition;
    private readonly float _returnThreshold;

    public ReturnState(EnemyStateMachine stateMachine, EnemyMovement movement, Transform transform, 
                        Vector2 startPosition, float returnThreshold) : base(stateMachine, movement, transform)
    {
        _startPosition = startPosition;
        _returnThreshold = returnThreshold;
    }

    public override void Enter()
    {
        float direction = Mathf.Sign(_startPosition.x - _transform.position.x);
        _movement.Move(direction);
        _movement.Flip(direction);
    }

    public override void Update()
    {
        float distanceToStart = Vector2.Distance(_transform.position, _startPosition);

        if (distanceToStart < _returnThreshold)
        {
            CompleteState(EnemyStates.Patrolling);
        }
        else
        {
            float direction = Mathf.Sign(_startPosition.x - _transform.position.x);
            _movement.Move(direction);
            _movement.Flip(direction);
        }
    }

    public override void Exit()
    {
        _movement.Stop();
    }
}