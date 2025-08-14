using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : MonoBehaviour
{
    [Header("Базовые настройки")]
    [SerializeField] protected float _moveSpeed = 7f;
    [SerializeField] protected float _jumpForce = 15f;
    [SerializeField] protected float _climbSpeed = 5f;
    [SerializeField] protected float _crouchSpeed = 4f;
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected LayerMask _ladderLayer;
    [SerializeField] protected Transform _groundCheck;
    [SerializeField] protected Transform _ceilingCheck;

    [Header("Компоненты")]
    [SerializeField] protected Rigidbody2D _rigidbody;
    [SerializeField] protected Collider2D _mainCollider;
    [SerializeField] protected Collider2D _crouchCollider;

    public StateMachine StateMachine { get; protected set; }
    public EntityInputController Input { get; protected set; }
    public EntityMovementController Movement { get; protected set; }
    public EntityAnimationController Animation { get; protected set; }
    public EntityFXController FX { get; protected set; }

    public bool IsFacingRight { get; protected set; } = true;
    public bool IsGrounded { get; protected set; }
    public bool IsTouchingLadder { get; protected set; }
    public bool CanStand { get; protected set; } = true;

    public float MoveSpeed => _moveSpeed;
    public float JumpForce => _jumpForce;
    public float ClimbSpeed => _climbSpeed;
    public float CrouchSpeed => _crouchSpeed;
    public Rigidbody2D Rigidbody2D => _rigidbody;

    protected virtual void Awake()
    {
        StateMachine = new StateMachine();
        Input = GetComponent<EntityInputController>();
        Movement = GetComponent<EntityMovementController>();
        Animation = GetComponent<EntityAnimationController>();
        FX = GetComponent<EntityFXController>();
    }

    protected virtual void Update()
    {
        StateMachine.CurrentState.UpdateState();
        UpdateChecks();
    }

    protected virtual void FixedUpdate()
    {
        StateMachine.CurrentState.FixedUpdateState();
    }

    protected virtual void UpdateChecks()
    {
        IsGrounded = Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _groundLayer);
        IsTouchingLadder = Physics2D.OverlapCircle(transform.position, 0.1f, _ladderLayer);

        if (_ceilingCheck)
            CanStand = !Physics2D.OverlapCircle(_ceilingCheck.position, 0.1f, _groundLayer);
    }

    public void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(int damage)
    {
        FX.StartBlink();
    }
}