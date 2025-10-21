using System;
using System.Collections.Generic;

public static class EnemyStateMachineFactory
{
    public static StateMachine Create(EnemyStateContext context, bool isStartFacingRight)
    {
        var states = new Dictionary<Type, IExitableState>
        {
            [typeof(EnemyPatrolState)] = new EnemyPatrolState(context, isStartFacingRight),
            [typeof(EnemyChaseState)] = new EnemyChaseState(context),
            [typeof(EnemyReturnState)] = new EnemyReturnState(context)
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