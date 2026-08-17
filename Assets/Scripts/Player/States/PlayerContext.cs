using UnityEngine;

namespace BattleForPlatforms.Player.States
{
    /// <summary>
    /// Контекст состояний игрока.
    /// Содержит все необходимые зависимости для работы состояний игрока.
    /// </summary>
    public class PlayerContext
    {
        public IMovable Movable { get; }
        public IAnimated Animator { get; }
        public IHealth Health { get; }
        public Input.InputReader InputReader { get; }
        public Move.Flipper Flipper { get; }
        public StateMachine.StateMachine StateMachine { get; set; }

        public PlayerContext(
            IMovable movable,
            IAnimated animator,
            IHealth health,
            Input.InputReader inputReader,
            Move.Flipper flipper)
        {
            Movable = movable;
            Animator = animator;
            Health = health;
            InputReader = inputReader;
            Flipper = flipper;
        }
    }
}
