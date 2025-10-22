using System;
using System.Collections.Generic;
using TMPro;

public class StateMachine : IStateChanger
{
    private IExitableState _currentState;
    private Dictionary<Type, IExitableState> _states = new();

    public StateMachine(Dictionary<Type, IExitableState> states)
    {
        _states = states;
    }

    public void ChangeState<TState>() where TState : class, IEnterableState
    {
        _currentState?.Exit();

        var state = GetState<TState>();
        _currentState = state;
        state.Enter();
    }

    public void ChangeState<TState, TPayload>(TPayload payload) where TState : class, IEnterablePayloadState<TPayload>
    {
        _currentState?.Exit();

        var state = GetState<TState>();
        _currentState = state;
        state.Enter(payload);
    }

    private TState GetState<TState>() where TState : class, IExitableState
    {
        return _states[typeof(TState)] as TState;
    }

    public void Update(float deltaTime)
    {
        if (_currentState is IUpdatableState updatableState)
            updatableState.Update(deltaTime);
    }

    public void FixedUpdate(float deltaTime)
    {
        if (_currentState is IFixedUpdatableState fixedUpdatableState)
            fixedUpdatableState.FixedUpdate(deltaTime);
    }
}