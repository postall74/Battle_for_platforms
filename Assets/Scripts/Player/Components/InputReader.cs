using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public const string Horizontal = "Horizontal";

    public float HorizontalDirection { get; private set; }
    public bool WasJumpPressed { get; private set; }
    public bool WasAttackKeyPressed { get; private set; }

    private void Update()
    {
        HorizontalDirection = Input.GetAxis(Horizontal);
        WasJumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
        WasAttackKeyPressed = Keyboard.current.eKey.wasPressedThisFrame;
    }
}