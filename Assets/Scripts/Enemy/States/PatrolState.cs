using UnityEngine;

public class PatrolState : EnemyState
{
    private readonly Transform _leftPatrolPoint;
    private readonly Transform _rightPatrolPoint;
    private readonly Flipper _flipper;
    private float _turnBuffer = 0.1f;

    public PatrolState(EnemyStateMachine stateMachine, EnemyMovement movement, Transform transform,
                       Transform leftPatrolPoint, Transform rightPatrolPoint,  Flipper flipper, bool startFacingRight) : base(stateMachine, movement, transform)
    {
        _leftPatrolPoint = leftPatrolPoint;
        _rightPatrolPoint = rightPatrolPoint;
        _flipper = flipper;
    }

    public override void Update()
    {
        if (_flipper.IsFacingRight && Transform.position.x + _turnBuffer >= _rightPatrolPoint.position.x)
            TurnAround(false);
        else if (_flipper.IsFacingRight == false && Transform.position.x - _turnBuffer <= _leftPatrolPoint.position.x)
            TurnAround(true);

        float currentDirection = _flipper.IsFacingRight ? 1 : -1;
        Movement.Move(currentDirection);

    }

    public override void Enter()
    {
        UpdateMovement();
    }

    public override void Exit()
    {
        Movement.Stop();
    }

    private void UpdateMovement()
    {
        float direction = _flipper.IsFacingRight ? 1 : -1;
        Movement.Move(direction);
    }

    private void TurnAround(bool shouldFaceRight)
    {
        float newDirection = shouldFaceRight ? 1 : -1;
        Vector3 newPosition = Transform.position;

        if (shouldFaceRight)
            newPosition.x = Mathf.Min(newPosition.x, _rightPatrolPoint.position.x - _turnBuffer);
        else
            newPosition.x = Mathf.Max(newPosition.x, _leftPatrolPoint.position.x + _turnBuffer);

        Transform.position = newPosition;
        Movement.Move(newDirection);
    }
}