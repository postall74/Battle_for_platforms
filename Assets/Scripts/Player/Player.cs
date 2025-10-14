using UnityEngine;

[RequireComponent(typeof(PlayerAnimator), typeof(CharacterMovement), typeof(Collector))]
public class Player : MonoBehaviour
{
    public Rigidbody2D Rigidbody { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public InputReader InputReader { get; private set; }
    public CharacterMovement Movement { get; private set; }
    public PlayerAnimator Animator { get; private set; }
    public Collector Collector { get; private set; }
    public Flipper Flipper { get; private set; }
    public GroundChecker GroundChecker { get; private set; }

    private void Awake()
    {
        InitializeComponents();
        SubscribeToEvents();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void InitializeComponents()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InputReader = GetComponent<InputReader>();
        Movement = GetComponent<CharacterMovement>();
        Animator = GetComponent<PlayerAnimator>();
        Collector = GetComponent<Collector>();
        Flipper = GetComponent<Flipper>();
        GroundChecker = GetComponent<GroundChecker>();
    }

    private void SubscribeToEvents()
    {
        GroundChecker.GroundedChanged += Animator.HandleGroundedChanged;
        Movement.Movement += Animator.HandleMovement;
        Movement.Jumped += Animator.HandleJump;
    }

    private void UnsubscribeFromEvents()
    {
        if (GroundChecker != null)
            GroundChecker.GroundedChanged -= Animator.HandleGroundedChanged;

        if (Movement != null)
        {
            Movement.Movement -= Animator.HandleMovement;
            Movement.Jumped -= Animator.HandleJump;
        }
    }

    private void HandleMovement()
    {
        Movement.Move(InputReader.HorizontalDirection);
        Animator.HandleVerticalVelocity(Movement.GetVerticalVelocity());
    }

    private void HandleInput()
    {
        if (InputReader.WasJumpPressed)
            Movement.Jump();
    }
}