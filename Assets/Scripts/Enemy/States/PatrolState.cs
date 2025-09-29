using UnityEngine;

public class PatrolState : EnemyState
{
    private readonly Transform _leftPatrolPoint;
    private readonly Transform _rightPatrolPoint;
    private bool _isFacingRight;
    private float _turnBuffer = 0.5f;

    public PatrolState(EnemyStateMachine stateMachine, EnemyMovement movement, Transform transform,
                       Transform leftPatrolPoint, Transform rightPatrolPoint, bool startFacingRight) : base(stateMachine, movement, transform)
    {
        _leftPatrolPoint = leftPatrolPoint;
        _rightPatrolPoint = rightPatrolPoint;
        _isFacingRight = startFacingRight;
    }

    public override void Enter()
    {
        _movement.Move(_isFacingRight ? 1 : -1);
    }

    public override void Update()
    {
        bool reachedRightPoint = Physics2D.OverlapArea(
            new Vector2(_rightPatrolPoint.position.x - _turnBuffer, _rightPatrolPoint.position.y - _turnBuffer),
            new Vector2(_rightPatrolPoint.position.x + _turnBuffer, _rightPatrolPoint.position.y + _turnBuffer)
        );
        bool reachedLeftPoint = Physics2D.OverlapArea(
            new Vector2(_leftPatrolPoint.position.x - _turnBuffer, _leftPatrolPoint.position.y - _turnBuffer),
            new Vector2(_leftPatrolPoint.position.x + _turnBuffer, _leftPatrolPoint.position.y + _turnBuffer)
        );


        if (_isFacingRight && reachedRightPoint)
            TurnAround(false);
        else if (!_isFacingRight && reachedLeftPoint)
            TurnAround(true);

        float currentDirection = _isFacingRight ? 1 : -1;
        _movement.Move(currentDirection);
    }

    public override void Exit()
    {
        _movement.Stop();
    }

    private void TurnAround(bool isFacingRight)
    {
        _isFacingRight = isFacingRight;
        float newDirection = _isFacingRight ? 1 : -1;
        _movement.Flip(newDirection);
        _movement.Stop();
    }
}