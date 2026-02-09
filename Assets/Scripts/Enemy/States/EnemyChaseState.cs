using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    private const float MAX_CHASE_TIME = 10f;

    private float _chaseStartTime;
    private float _lastPlayerPositionUpdateTime;
    private Vector2 _lastKnowPlayerPosition;
    
    public EnemyChaseState(EnemyStateContext context)
        : base(context) { }

    public override void Enter()
    {
        _chaseStartTime = Time.time;
        _lastPlayerPositionUpdateTime = Time.time;

        if (Context.Player != null)
        { 
            _lastKnowPlayerPosition = Context.Player.position;
            UpdateMovement();
        }
    }

    public override void Update(float deltaTime)
    {
        bool isPlayerVisible = IsPlayerVisible();

        if (isPlayerVisible)
        {
            _lastKnowPlayerPosition = Context.Player.position;
            _lastPlayerPositionUpdateTime = Time.time;

            if(IsPlayerInAttackRange())
            {
                StateChanger.ChangeState<EnemyAttackState>();
                return;
            }

            UpdateMovement();
        }
        else
        {
            float timeSinceLastSighting = Time.time - _lastPlayerPositionUpdateTime;

            if (timeSinceLastSighting > 2f || Time.time - _chaseStartTime > MAX_CHASE_TIME)
            {
                StateChanger.ChangeState<EnemyReturnState>();
                return;
            }
        }

        UpdateMovementToLastKnownPosition();
    }

    public override void Exit()
    {
        Context.Movement.Stop();
    }

    private void UpdateMovement()
    {
        if (Context.Player == null)
            return;

        float direction = Mathf.Sign(Context.Player.position.x - Context.Transform.position.x);
        Context.Movement.Move(direction);
    }

    private void UpdateMovementToLastKnownPosition()
    {
        float direction = Mathf.Sign(_lastKnowPlayerPosition.x - Context.Transform.position.x);
        Context.Movement.Move(direction);

        float distance = Vector2.Distance(Context.Transform.position, _lastKnowPlayerPosition);

        if (distance < 0.5f)
            StateChanger.ChangeState<EnemyReturnState>();
    }

    private bool IsPlayerInAttackRange()
    {
        if (Context.Player == null)
            return false;

        float distance = Vector2.Distance(Context.Transform.position, Context.Player.position);
        return distance <= Context.Attacker.AttackRange;
    }
}