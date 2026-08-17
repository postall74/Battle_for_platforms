using UnityEngine;

namespace BattleForPlatforms.Enemy.States
{
    /// <summary>
    /// Контекст состояний врага.
    /// Содержит все необходимые зависимости для работы состояний врага.
    /// </summary>
    public class EnemyContext
    {
        public IMovable Movable { get; }
        public IAnimated Animator { get; }
        public IHealth Health { get; }
        public Combat.IContactAttacker Attacker { get; }
        public Providers.PatrolZone PatrolZone { get; }
        public Providers.EnemyVision Vision { get; }
        public Transform PlayerTransform { get; set; }
        public StateMachine.StateMachine StateMachine { get; set; }
        public Transform StartPosition { get; }

        public EnemyContext(
            IMovable movable,
            IAnimated animator,
            IHealth health,
            Combat.IContactAttacker attacker,
            Providers.PatrolZone patrolZone,
            Providers.EnemyVision vision,
            Transform playerTransform,
            Transform startPosition)
        {
            Movable = movable;
            Animator = animator;
            Health = health;
            Attacker = attacker;
            PatrolZone = patrolZone;
            Vision = vision;
            PlayerTransform = playerTransform;
            StartPosition = startPosition;
        }
    }
}
