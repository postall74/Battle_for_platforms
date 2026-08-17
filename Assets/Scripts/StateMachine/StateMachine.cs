using System;
using System.Collections.Generic;

namespace BattleForPlatforms.StateMachine
{
    /// <summary>
    /// Универсальная машина состояний.
    /// Управляет переключением между состояниями, вызывая методы Enter, Exit, Update и FixedUpdate.
    /// </summary>
    public class StateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;

        /// <summary>
        /// Конструктор машины состояний.
        /// </summary>
        /// <param name="states">Словарь состояний, где ключ - тип состояния, значение - экземпляр состояния.</param>
        public StateMachine(Dictionary<Type, IExitableState> states)
        {
            _states = states ?? throw new ArgumentNullException(nameof(states));
        }

        /// <summary>
        /// Инициализация машины состояний с запуском начального состояния.
        /// </summary>
        /// <typeparam name="T">Тип начального состояния.</typeparam>
        public void Initialize<T>() where T : IEnterableState
        {
            var stateType = typeof(T);
            
            if (!_states.ContainsKey(stateType))
                throw new ArgumentException($"State {stateType.Name} not found in state machine", nameof(T));

            _currentState = _states[stateType];
            ((IEnterableState)_currentState).Enter();
        }

        /// <summary>
        /// Переключение на новое состояние.
        /// </summary>
        /// <typeparam name="T">Тип нового состояния.</typeparam>
        public void ChangeState<T>() where T : IEnterableState
        {
            var stateType = typeof(T);
            
            if (!_states.ContainsKey(stateType))
                throw new ArgumentException($"State {stateType.Name} not found in state machine", nameof(T));

            if (_currentState != null)
                _currentState.Exit();

            _currentState = _states[stateType];
            ((IEnterableState)_currentState).Enter();
        }

        /// <summary>
        /// Переключение на новое состояние с передачей параметра.
        /// </summary>
        /// <typeparam name="T">Тип нового состояния.</typeparam>
        /// <param name="payload">Параметр для передачи в состояние.</param>
        public void ChangeState<T>(object payload) where T : IEnterablePayloadState<object>
        {
            var stateType = typeof(T);
            
            if (!_states.ContainsKey(stateType))
                throw new ArgumentException($"State {stateType.Name} not found in state machine", nameof(T));

            if (_currentState != null)
                _currentState.Exit();

            _currentState = _states[stateType];
            ((IEnterablePayloadState<object>)_currentState).Enter(payload);
        }

        /// <summary>
        /// Обновление текущего состояния (вызывается каждый кадр).
        /// </summary>
        public void Update()
        {
            if (_currentState is IUpdatableState updatableState)
                updatableState.Update();
        }

        /// <summary>
        /// Фиксированное обновление текущего состояния (вызывается в фиксированный шаг времени).
        /// </summary>
        public void FixedUpdate()
        {
            if (_currentState is IFixedUpdatableState fixedUpdatableState)
                fixedUpdatableState.FixedUpdate();
        }
    }
}
