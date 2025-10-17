using System;
using System.Collections.Generic;

public class StateMachine
{
    private IState _currentState;
    private Dictionary<Type, IState> _states = new();

    public event Action<IState> StateChanged;

    public void AddState<T>(T state) where T : IState
    {
        _states[typeof(T)] = state;
    }

    public void ChangeState<T>() where T : IState
    {
        _currentState?.Exit();

        if (_states.TryGetValue(typeof(T), out IState newState))
        {
            _currentState = newState;
            _currentState.Enter();
            StateChanged?.Invoke(_currentState);
        }
    }

    public void Update()
    {
        _currentState?.Update();
    }

    public void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }
}