using UnityEngine;

public class PatrolState : EnemyState
{
    private readonly Transform _leftPatrolPoint;
    private readonly Transform _rightPatrolPoint;
    private bool _isFacingRight;
    private float _turnBuffer = 0.1f;

    public PatrolState(EnemyStateMachine stateMachine, EnemyMovement movement, Transform transform,
                       Transform leftPatrolPoint, Transform rightPatrolPoint, bool startFacingRight) : base(stateMachine, movement, transform)
    {
        _leftPatrolPoint = leftPatrolPoint;
        _rightPatrolPoint = rightPatrolPoint;
        _isFacingRight = startFacingRight;
    }

    public override void Update()
    {
        if (_isFacingRight && Transform.position.x + _turnBuffer >= _rightPatrolPoint.position.x)
            TurnAround(false);
        else if (_isFacingRight == false && Transform.position.x - _turnBuffer <= _leftPatrolPoint.position.x)
            TurnAround(true);

        float currentDirection = _isFacingRight ? 1 : -1;
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
        float direction = _isFacingRight ? 1 : -1;
        Movement.Move(direction);
        Movement.Flip(direction);
    }

    private void TurnAround(bool isFacingRight)
    {
        _isFacingRight = isFacingRight;
        float newDirection = _isFacingRight ? 1 : -1;

        Vector3 newPosition = Transform.position;

        if (_isFacingRight)
            newPosition.x = Mathf.Min(newPosition.x, _rightPatrolPoint.position.x - _turnBuffer);
        else
            newPosition.x = Mathf.Max(newPosition.x, _leftPatrolPoint.position.x + _turnBuffer);

        Transform.position = newPosition;
        Movement.Flip(newDirection);
    }
}