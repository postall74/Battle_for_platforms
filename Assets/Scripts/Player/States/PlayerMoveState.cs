using BattleForPlatforms.Interfaces.States;

namespace BattleForPlatforms.Player.States
{
    /// <summary>
    /// Состояние движения игрока.
    /// Обрабатывает перемещение и прыжки.
    /// </summary>
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerContext context) : base(context)
        {
        }

        /// <summary>
        /// Вход в состояние движения.
        /// </summary>
        public override void Enter()
        {
            Context.Animator?.SetHit(false);
        }

        /// <summary>
        /// Обновление состояния движения каждый кадр.
        /// </summary>
        public override void Update()
        {
            HandleInput();
            CheckForJump();
            UpdateAnimation();
        }

        /// <summary>
        /// Фиксированное обновление состояния движения.
        /// </summary>
        public override void FixedUpdate()
        {
            HandleMovement();
        }

        /// <summary>
        /// Обработка ввода игрока.
        /// </summary>
        private void HandleInput()
        {
            if (Context.InputReader == null) return;

            float moveDirection = Context.InputReader.MoveDirection;
            
            // Разворот персонажа
            if (moveDirection != 0 && Context.Flipper != null)
            {
                Context.Flipper.Flip(moveDirection);
            }
        }

        /// <summary>
        /// Проверка попытки прыжка.
        /// </summary>
        private void CheckForJump()
        {
            if (Context.InputReader?.JumpPressed == true && Context.Movable?.IsGrounded() == true)
            {
                ChangeToJumpState();
            }
        }

        /// <summary>
        /// Обработка движения.
        /// </summary>
        private void HandleMovement()
        {
            if (Context.Movable == null) return;

            float moveDirection = Context.InputReader?.MoveDirection ?? 0f;
            Context.Movable.Move(moveDirection);
        }

        /// <summary>
        /// Обновление анимации.
        /// </summary>
        private void UpdateAnimation()
        {
            if (Context.Animator == null) return;

            float moveSpeed = Mathf.Abs(Context.InputReader?.MoveDirection ?? 0f);
            bool isInAir = !Context.Movable?.IsGrounded() ?? true;

            Context.Animator.SetMoveSpeed(moveSpeed);
            Context.Animator.SetInAir(isInAir);
        }
    }
}
