using BattleForPlatforms.Interfaces.States;

namespace BattleForPlatforms.Player.States
{
    /// <summary>
    /// Состояние получения урона игроком.
    /// Воспроизводит анимацию получения урона и переключается обратно на движение.
    /// </summary>
    public class PlayerDamageState : PlayerBaseState
    {
        private float _damageTimer;
        private const float DamageDuration = 0.5f;

        public PlayerDamageState(PlayerContext context) : base(context)
        {
        }

        /// <summary>
        /// Вход в состояние получения урона.
        /// Воспроизводит анимацию и запускает таймер.
        /// </summary>
        public override void Enter()
        {
            _damageTimer = DamageDuration;
            Context.Animator?.SetHit(true);
        }

        /// <summary>
        /// Выход из состояния получения урона.
        /// </summary>
        public override void Exit()
        {
            Context.Animator?.SetHit(false);
        }

        /// <summary>
        /// Обновление состояния получения урона каждый кадр.
        /// Отсчитывает время и переключается на движение.
        /// </summary>
        public override void Update()
        {
            _damageTimer -= Time.deltaTime;

            if (_damageTimer <= 0)
            {
                ChangeToMoveState();
            }
        }
    }
}
