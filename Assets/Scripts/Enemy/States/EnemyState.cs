using System;
using UnityEngine;

public abstract class EnemyState
{
    protected EnemyStateMachine _stateMachine;
    protected EnemyMovement _movement;
    protected Transform _transform;

    public event Action<EnemyStates> OnStateCompleted; 

    protected EnemyState (EnemyStateMachine stateMachine, EnemyMovement movement, Transform transform)
    {
        _stateMachine = stateMachine;
        _movement = movement;
        _transform = transform;
    }

    protected void CompleteState(EnemyStates nextState)
    {
        OnStateCompleted?.Invoke(nextState);
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}