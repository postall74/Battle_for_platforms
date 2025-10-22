using UnityEditor.SceneManagement;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private bool _isFacingRight;
    private float _turnBuffer = 0.1f;

    public EnemyPatrolState(EnemyStateContext context, bool startFacingRight)
        : base(context)
    {
        _isFacingRight = startFacingRight;
    }

    public override void Enter()
    {
        Context.Movement.Move(GetDirection());
    }

    public override void Update(float deltaTime)
    {
        if (IsPlayerVisible())
        {
            StateChanger.ChangeState<EnemyChaseState>();
            return;
        }

        if (_isFacingRight && Context.Transform.position.x + _turnBuffer >= Context.RightPatrolPoint.position.x)
            TurnArround(false);
        else if (_isFacingRight == false && Context.Transform.position.x - _turnBuffer <= Context.LeftPatrolPoint.position.x)
            TurnArround(true);

        Context.Movement.Move(GetDirection());
    }

    public override void Exit()
    {
        Context.Movement.Stop();
    }

    private void TurnArround(bool isFacingRight)
    {
        _isFacingRight = isFacingRight;
        float newDirection = GetDirection();

        Vector3 newPosition = Context.Transform.position;

        if(_isFacingRight)
            newPosition.x = Mathf.Min(newPosition.x, Context.RightPatrolPoint.position.x - _turnBuffer);
        else
            newPosition.x = Mathf.Max(newPosition.x, Context.LeftPatrolPoint.position.x + _turnBuffer);

        Context.Transform.position = newPosition;
        Context.Movement.Move(newDirection);


    }

    private float GetDirection()
    {
        return _isFacingRight ? 1 : -1;
    }
}