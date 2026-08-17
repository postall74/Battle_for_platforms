using UnityEngine;

namespace BattleForPlatforms.Input
{
    /// <summary>
    /// Класс для чтения ввода игрока.
    /// Предоставляет данные о вводе для управления персонажем.
    /// </summary>
    public class InputReader : MonoBehaviour
    {
        private PlayerControls _controls;

        /// <summary>
        /// Событие, вызываемое при изменении направления движения.
        /// </summary>
        public event System.Action<float> OnMoveChanged;

        /// <summary>
        /// Событие, вызываемое при нажатии кнопки прыжка.
        /// </summary>
        public event System.Action OnJumpPressed;

        private void Awake()
        {
            _controls = new PlayerControls();
            
            // Подписка на события ввода
            _controls.Player.Move.performed += ctx => OnMoveChanged?.Invoke(ctx.ReadValue<float>());
            _controls.Player.Move.canceled += ctx => OnMoveChanged?.Invoke(0f);
            _controls.Player.Jump.performed += ctx => OnJumpPressed?.Invoke();
        }

        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        /// <summary>
        /// Текущее направление движения (-1, 0, 1).
        /// </summary>
        public float MoveDirection { get; private set; }

        /// <summary>
        /// Флаг, указывающий, была ли нажата кнопка прыжка.
        /// </summary>
        public bool JumpPressed { get; private set; }

        private void Update()
        {
            // Сброс флага прыжка после обработки
            if (JumpPressed)
                JumpPressed = false;
        }

        private void OnMove(float direction)
        {
            MoveDirection = direction;
            OnMoveChanged?.Invoke(direction);
        }

        private void OnJump()
        {
            JumpPressed = true;
            OnJumpPressed?.Invoke();
        }
    }
}
