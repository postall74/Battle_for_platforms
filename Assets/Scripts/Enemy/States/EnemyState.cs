using System;
using UnityEngine;

public abstract class EnemyState
{
    protected EnemyStateMachine StateMachine;
    protected EnemyMovement Movement;
    protected Transform Transform;

    public event Action<EnemyStateType> StateCompleted; 

    protected EnemyState (EnemyStateMachine stateMachine, EnemyMovement movement, Transform transform)
    {
        StateMachine = stateMachine;
        Movement = movement;
        Transform = transform;
    }

    protected void CompleteState(EnemyStateType nextState)
    {
        StateCompleted?.Invoke(nextState);
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}