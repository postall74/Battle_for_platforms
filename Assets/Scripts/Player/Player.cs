using UnityEngine;
using BattleForPlatforms.Player.States;

namespace BattleForPlatforms.Player
{
    /// <summary>
    /// Главный компонент игрока.
    /// Инициализирует все зависимости и машину состояний.
    /// </summary>
    public class Player : MonoBehaviour
    {
        [Header("Компоненты")]
        [SerializeField] private Move.CharacterMovement _movement;
        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private Health.HealthProvider _health;
        [SerializeField] private Input.InputReader _inputReader;
        [SerializeField] private Move.Flipper _flipper;

        private PlayerStateMachine _stateMachine;
        private PlayerContext _context;

        private void Awake()
        {
            // Получаем компоненты, если они не назначены
            if (_movement == null)
                _movement = GetComponent<Move.CharacterMovement>();
            
            if (_animator == null)
                _animator = GetComponent<PlayerAnimator>();
            
            if (_health == null)
                _health = GetComponent<Health.HealthProvider>();
            
            if (_inputReader == null)
                _inputReader = GetComponent<Input.InputReader>();
            
            if (_flipper == null)
                _flipper = GetComponent<Move.Flipper>();

            // Создаем контекст
            _context = new PlayerContext(
                _movement,
                _animator,
                _health,
                _inputReader,
                _flipper
            );

            // Создаем машину состояний
            _stateMachine = new PlayerStateMachine(_context);
            _context.StateMachine = _stateMachine;

            // Подписываемся на события здоровья
            _health.OnDied += HandleDeath;
            _health.OnHealthChanged += HandleHealthChanged;
        }

        private void Start()
        {
            // Инициализируем машину состояний с начальным состоянием
            _stateMachine.Initialize<PlayerMoveState>();
        }

        private void Update()
        {
            if (_health.IsAlive)
            {
                _stateMachine.Update();
            }
        }

        private void FixedUpdate()
        {
            if (_health.IsAlive)
            {
                _stateMachine.FixedUpdate();
            }
        }

        /// <summary>
        /// Обработка смерти игрока.
        /// </summary>
        private void HandleDeath()
        {
            _stateMachine.ChangeState<PlayerDeathState>();
        }

        /// <summary>
        /// Обработка изменения здоровья.
        /// </summary>
        private void HandleHealthChanged(float currentHealth, float maxHealth)
        {
            // Можно добавить логику для UI или эффектов
        }

        private void OnDestroy()
        {
            if (_health != null)
            {
                _health.OnDied -= HandleDeath;
                _health.OnHealthChanged -= HandleHealthChanged;
            }
        }
    }
}
