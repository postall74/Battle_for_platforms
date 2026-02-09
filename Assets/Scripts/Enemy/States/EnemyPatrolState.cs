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
        float direction = GetDirection();
        Context.Movement.Move(direction);
    }

    public override void Update(float deltaTime)
    {
        if (IsPlayerVisible())
        {
            StateChanger.ChangeState<EnemyChaseState>();
            return;
        }

        if (_isFacingRight && Context.Transform.position.x >= Context.RightPatrolPoint.position.x - _turnBuffer)
            TurnAround(false); 
        else if (!_isFacingRight && Context.Transform.position.x <= Context.LeftPatrolPoint.position.x + _turnBuffer)
            TurnAround(true); 

        float direction = GetDirection();
        Context.Movement.Move(direction);
    }

    public override void Exit()
    {
        Context.Movement.Stop();
    }

    private void TurnAround(bool isFacingRight) 
    {
        _isFacingRight = isFacingRight;
        Context.Movement.Stop();
    }

    private float GetDirection()
    {
        return _isFacingRight ? 1 : -1;
    }
}