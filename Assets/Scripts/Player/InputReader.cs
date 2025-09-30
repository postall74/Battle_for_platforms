using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public const string Horizontal = "Horizontal";

    public float HorizontalDirection { get; private set; }
    public bool JumpPressed { get; private set; }

    private void Update()
    {
        HorizontalDirection = Input.GetAxis(Horizontal);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            JumpPressed = true;
    }

    public bool WasKeyJumpPressed()
    {
        if(JumpPressed)
        {
            JumpPressed = false;
            return true;
        }    

        return false;
    }
}