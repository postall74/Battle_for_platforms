using System;
using System.Collections.Generic;

public class EnemyStateMachineFactory : IEnemyStateMachineFactory
{
    public StateMachine Create(EnemyStateContext context, bool startFacingRight)
    {
        var states = new Dictionary<Type, IExitableState>
        {
            [typeof(EnemyPatrolState)] = new EnemyPatrolState(context, startFacingRight),
            [typeof(EnemyChaseState)] = new EnemyChaseState(context),
            [typeof(EnemyReturnState)] = new EnemyReturnState(context),
            [typeof(EnemyAttackState)] = new EnemyAttackState(context),
            [typeof(EnemyDamageState)] = new EnemyDamageState(context)
        };

        var stateMachine = new StateMachine(states);

        foreach (var state in states.Values)
        {
            if (state is EnemyBaseState enemyState)
                enemyState.SetStateMachine(stateMachine);
        }

        return stateMachine;
    }
}