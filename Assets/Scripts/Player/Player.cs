using UnityEngine;

[RequireComponent(typeof(PlayerAnimation), typeof(Mover))]
public class Player : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public InputReader InputReader { get; private set; }
    public Mover Mover { get; private set; }
    public PlayerAnimation Animation { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InputReader = GetComponent<InputReader>();
        Mover = GetComponent<Mover>();
        Animation = GetComponent<PlayerAnimation>();
    }

    private void FixedUpdate()
    {
        Mover.Move(InputReader.HorizontalDirection);
        Animation.PlayAnimationRun(InputReader.HorizontalDirection);

        if (InputReader.WasKeyJumpPressed())
        {
            Mover.Jump();
            Animation.TriggerJump();
        }

        Animation.PlayAnimationJumpOrFall(Mover.GetVerticalVelocity(), Mover.IsGrounded());
    }
}