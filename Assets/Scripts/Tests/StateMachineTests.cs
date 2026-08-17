using NUnit.Framework;
using UnityEngine;

/// <summary>
/// Тесты для машины состояний StateMachine.
/// Проверяют корректность переключения между состояниями и вызова методов Enter/Exit/Update.
/// </summary>
[TestFixture]
public class StateMachineTests
{
    private StateMachine _stateMachine;
    
    private class TestStateEnterable : IEnterableState, IExitableState
    {
        public bool Entered { get; private set; }
        public bool Exited { get; private set; }
        
        public void Enter() => Entered = true;
        public void Exit() => Exited = true;
    }
    
    private class TestStateUpdatable : IEnterableState, IExitableState, IUpdatableState
    {
        public bool Entered { get; private set; }
        public bool Exited { get; private set; }
        public bool Updated { get; private set; }
        public float LastDeltaTime { get; private set; }
        
        public void Enter() => Entered = true;
        public void Exit() => Exited = true;
        public void Update(float deltaTime) 
        { 
            Updated = true;
            LastDeltaTime = deltaTime;
        }
    }
    
    private class TestStateFixedUpdatable : IEnterableState, IExitableState, IFixedUpdatableState
    {
        public bool Entered { get; private set; }
        public bool Exited { get; private set; }
        public bool FixedUpdated { get; private set; }
        public float LastDeltaTime { get; private set; }
        
        public void Enter() => Entered = true;
        public void Exit() => Exited = true;
        public void FixedUpdate(float deltaTime) 
        { 
            FixedUpdated = true;
            LastDeltaTime = deltaTime;
        }
    }

    [SetUp]
    public void Setup()
    {
        var states = new System.Collections.Generic.Dictionary<System.Type, IExitableState>
        {
            [typeof(TestStateEnterable)] = new TestStateEnterable(),
            [typeof(TestStateUpdatable)] = new TestStateUpdatable(),
            [typeof(TestStateFixedUpdatable)] = new TestStateFixedUpdatable()
        };
        
        _stateMachine = new StateMachine(states);
    }

    /// <summary>
    /// Проверка что состояние входит при переключении.
    /// </summary>
    [Test]
    public void ChangeState_ShouldCallEnterOnNewState()
    {
        _stateMachine.ChangeState<TestStateEnterable>();
        
        var state = GetPrivateField<IExitableState>(_stateMachine, "_currentState") as TestStateEnterable;
        Assert.IsTrue(state.Entered);
    }

    /// <summary>
    /// Проверка что предыдущее состояние выходит при переключении.
    /// </summary>
    [Test]
    public void ChangeState_ShouldCallExitOnPreviousState()
    {
        _stateMachine.ChangeState<TestStateEnterable>();
        var previousState = GetPrivateField<IExitableState>(_stateMachine, "_currentState") as TestStateEnterable;
        
        _stateMachine.ChangeState<TestStateUpdatable>();
        
        Assert.IsTrue(previousState.Exited);
    }

    /// <summary>
    /// Проверка что Update вызывает обновление текущего состояния.
    /// </summary>
    [Test]
    public void Update_ShouldCallUpdateOnCurrentState()
    {
        _stateMachine.ChangeState<TestStateUpdatable>();
        
        _stateMachine.Update(0.016f);
        
        var state = GetPrivateField<IExitableState>(_stateMachine, "_currentState") as TestStateUpdatable;
        Assert.IsTrue(state.Updated);
        Assert.AreEqual(0.016f, state.LastDeltaTime, 0.001f);
    }

    /// <summary>
    /// Проверка что Update не вызывает обновление у состояний без интерфейса IUpdatableState.
    /// </summary>
    [Test]
    public void Update_ShouldNotCallUpdateOnNonUpdatableState()
    {
        _stateMachine.ChangeState<TestStateEnterable>();
        
        // Не должно вызывать исключений
        _stateMachine.Update(0.016f);
        
        Assert.Pass("Update не вызвал исключений на состоянии без IUpdatableState");
    }

    /// <summary>
    /// Проверка что FixedUpdate вызывает обновление текущего состояния.
    /// </summary>
    [Test]
    public void FixedUpdate_ShouldCallFixedUpdateOnCurrentState()
    {
        _stateMachine.ChangeState<TestStateFixedUpdatable>();
        
        _stateMachine.FixedUpdate(0.02f);
        
        var state = GetPrivateField<IExitableState>(_stateMachine, "_currentState") as TestStateFixedUpdatable;
        Assert.IsTrue(state.FixedUpdated);
        Assert.AreEqual(0.02f, state.LastDeltaTime, 0.001f);
    }

    /// <summary>
    /// Проверка что FixedUpdate не вызывает обновление у состояний без интерфейса IFixedUpdatableState.
    /// </summary>
    [Test]
    public void FixedUpdate_ShouldNotCallFixedUpdateOnNonFixedUpdatableState()
    {
        _stateMachine.ChangeState<TestStateEnterable>();
        
        // Не должно вызывать исключений
        _stateMachine.FixedUpdate(0.02f);
        
        Assert.Pass("FixedUpdate не вызвал исключений на состоянии без IFixedUpdatableState");
    }

    /// <summary>
    /// Получение приватного поля через рефлексию.
    /// </summary>
    private T GetPrivateField<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (T)field?.GetValue(obj);
    }
}
