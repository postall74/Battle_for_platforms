// Заглушка для PlayerControls - в реальном проекте это генерируемый класс из Input System
namespace BattleForPlatforms.Input
{
    public class PlayerControls
    {
        public PlayerInputMap Player { get; } = new PlayerInputMap();

        public void Enable() { }
        public void Disable() { }
    }

    public class PlayerInputMap
    {
        public InputAction Move { get; } = new InputAction();
        public InputAction Jump { get; } = new InputAction();
    }

    public class InputAction
    {
        public event System.Action<InputActionContext> performed;
        public event System.Action<InputActionContext> canceled;

        public void Perform() => performed?.Invoke(new InputActionContext(1f));
        public void Cancel() => canceled?.Invoke(new InputActionContext(0f));
    }

    public struct InputActionContext
    {
        private readonly float _value;
        public InputActionContext(float value) => _value = value;
        public float ReadValue<T>() => _value;
    }
}
