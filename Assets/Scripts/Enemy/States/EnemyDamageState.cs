using UnityEngine;

public class EnemyDamageState : EnemyBaseState
{
    private const float STUN_DURATION = 0.5f;
    private float _enterTime;

    public EnemyDamageState(EnemyStateContext context) : base(context)
    { }

    public override void Enter()
    {
        _enterTime = Time.time;
        Context.Movement.Stop();
    }

    public override void Update(float deltaTime)
    {
        if (Time.time >= _enterTime + STUN_DURATION)
        {
            if (Context.Player != null && IsPlayerVisible())
                StateChanger.ChangeState<EnemyChaseState>();
            else
                StateChanger.ChangeState<EnemyReturnState>();
        }
    }

    public override void Exit()
    { }
}