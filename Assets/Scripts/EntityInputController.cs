using UnityEngine;

public class EntityInputController : MonoBehaviour
{
    public float MoveDirection { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool IsCrouching { get; private set; }
    public float VerticalDirection { get; private set; }

    private void Update()
    {
        MoveDirection = Input.GetAxisRaw("Horizontal");
        VerticalDirection = Input.GetAxisRaw("Vertical");
        JumpTriggered = Input.GetButtonDown("Jump");
        IsCrouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.S);
    }
}