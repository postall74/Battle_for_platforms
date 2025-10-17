using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private bool _isFacingRight;
    private float _turnBuffer = 0.1f;

    public EnemyPatrolState(EnemyStateContext context, StateMachine stateMachine, bool startFacingRight)
        : base(context, stateMachine)
    {
        _isFacingRight = startFacingRight;
    }

    public override void Enter()
    {
        float direction = _isFacingRight ? 1 : -1;
        Context.Movement.Move(direction);
    }

    public override void Update()
    {
        if (IsPlayerVisible())
        {
            StateMachine.ChangeState<EnemyChaseState>();
            return;
        }

        if (_isFacingRight && Context.Transform.position.x + _turnBuffer >= Context.RightPatrolPoint.position.x)
            TurnAround(false);
        else if (!_isFacingRight && Context.Transform.position.x - _turnBuffer <= Context.LeftPatrolPoint.position.x)
            TurnAround(true);

        float currentDirection = _isFacingRight ? 1 : -1;
        Context.Movement.Move(currentDirection);
    }

    public override void Exit()
    {
        Context.Movement.Stop();
    }

    private void TurnAround(bool isFacingRight)
    {
        _isFacingRight = isFacingRight;
        float newDirection = _isFacingRight ? 1 : -1;

        Vector3 newPosition = Context.Transform.position;

        if (_isFacingRight)
            newPosition.x = Mathf.Min(newPosition.x, Context.RightPatrolPoint.position.x - _turnBuffer);
        else
            newPosition.x = Mathf.Max(newPosition.x, Context.LeftPatrolPoint.position.x + _turnBuffer);

        Context.Transform.position = newPosition;

        Context.Movement.Move(newDirection);
    }
}