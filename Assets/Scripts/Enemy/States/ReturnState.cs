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
        UpdateMovement();
    }

    public override void Update()
    {
        float distanceToStart = Vector2.Distance(Transform.position, _startPosition);

        if (distanceToStart < _returnThreshold)
            CompleteState(EnemyStateType.Patrolling);
        else
            UpdateMovement();
    }

    public override void Exit()
    {
        Movement.Stop();
    }

    private void UpdateMovement()
    {
        float direction = Mathf.Sign(_startPosition.x - Transform.position.x);
        Movement.Move(direction);
    }
}