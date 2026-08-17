using BattleForPlatforms.Interfaces.States;

namespace BattleForPlatforms.Player.States
{
    /// <summary>
    /// Состояние смерти игрока.
    /// Воспроизводит анимацию смерти и завершает игру.
    /// </summary>
    public class PlayerDeathState : PlayerBaseState
    {
        public PlayerDeathState(PlayerContext context) : base(context)
        {
        }

        /// <summary>
        /// Вход в состояние смерти.
        /// Воспроизводит анимацию смерти.
        /// </summary>
        public override void Enter()
        {
            Context.Animator?.SetDead(true);
            
            // Отключаем физику и коллайзеры
            var rigidbody = (Context.Movable as UnityEngine.MonoBehaviour)?.GetComponent<UnityEngine<Rigidbody2D>();
            if (rigidbody != null)
            {
                rigidbody.velocity = UnityEngine.Vector2.zero;
                rigidbody.gravityScale = 0;
            }
        }

        /// <summary>
        /// Обновление состояния смерти.
        /// В данном состоянии игрок не управляется.
        /// </summary>
        public override void Update()
        {
            // Игрок мертв, управление отключено
        }
    }
}
