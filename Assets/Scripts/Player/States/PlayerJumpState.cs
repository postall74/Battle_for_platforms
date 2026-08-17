using BattleForPlatforms.Interfaces.States;

namespace BattleForPlatforms.Player.States
{
    /// <summary>
    /// Состояние прыжка игрока.
    /// Обрабатывает нахождение в воздухе и приземление.
    /// </summary>
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerContext context) : base(context)
        {
        }

        /// <summary>
        /// Вход в состояние прыжка.
        /// Выполняет прыжок и обновляет анимацию.
        /// </summary>
        public override void Enter()
        {
            Context.Movable?.Jump();
            Context.Animator?.SetInAir(true);
        }

        /// <summary>
        /// Обновление состояния прыжка каждый кадр.
        /// Проверяет приземление.
        /// </summary>
        public override void Update()
        {
            CheckForLanding();
            UpdateAnimation();
        }

        /// <summary>
        /// Проверка приземления.
        /// </summary>
        private void CheckForLanding()
        {
            if (Context.Movable?.IsGrounded() == true)
            {
                ChangeToMoveState();
            }
        }

        /// <summary>
        /// Обновление анимации.
        /// </summary>
        private void UpdateAnimation()
        {
            if (Context.Animator == null) return;

            float moveSpeed = Mathf.Abs(Context.InputReader?.MoveDirection ?? 0f);
            Context.Animator.SetMoveSpeed(moveSpeed);
        }
    }
}
