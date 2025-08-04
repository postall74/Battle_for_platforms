using UnityEngine;

[RequireComponent(typeof(Rigidbody), (typeof(Collider2D)))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _climbSpeed = 3f;

    [Header("Collision Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _ladderLayer;
    [SerializeField] private LayerMask _hazardLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _ladderCheck;
    [SerializeField] private Vector2 _groundCheckSize = new(0.5f, 0.5f);
    [SerializeField] private float _ladderCheckRadius = 0.2f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _mainCollider;
    [SerializeField] private PlayerAnimationController _animator;

    public Vector2 InputDirection { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsTouchingLadder { get; private set; }
    public bool IsAlive { get; private set; } = true;
    public PlayerAnimationController Animator => _animator;
    public Rigidbody2D Rigidbody => _rigidbody;
    public Collider2D Collider => _mainCollider;

    private PlayerStateMachine _stateMachine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _mainCollider = GetComponent<Collider2D>();
        _animator = GetComponent<PlayerAnimationController>();
        _stateMachine = new PlayerStateMachine();
    }

    private void Start()
    {
        //Инициализация начального состояния
        _stateMachine.Initialize(new IdleState(this, _stateMachine));
    }

    private void Update()
    {
        if (IsAlive == false)
            return;

        //обработка ввода
        InputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Проверка коллизий
        IsGrounded = Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize, 0, _groundLayer);
        IsTouchingLadder = Physics2D.OverlapCircle(_ladderCheck.position, _ladderCheckRadius, _ladderLayer);

        //Обновление состояния
        _stateMachine.Update();
        _animator.UpdateAnimations();
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_hazardLayer == (_hazardLayer | (1 << collision.gameObject.layer)))
            ApplyDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hazardLayer == (_hazardLayer | (1 << collision.gameObject.layer)))
            ApplyDamage();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheck.position, _groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_ladderCheck.position, _ladderCheckRadius);
    }

    public void Move(float speedMultiplier = 1f)
    {
        _rigidbody.linearVelocity = new Vector2(InputDirection.x * _moveSpeed * speedMultiplier, _rigidbody.linearVelocity.y);
    }

    public void Jump()
    {
        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _jumpForce);
    }

    public void Climb()
    {
        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, InputDirection.y * _climbSpeed);
        _rigidbody.gravityScale = 0;
    }

    public void StopClimbing()
    {
        _rigidbody.gravityScale = 3;
    }

    public void ApplyDamage()
    {
        if (IsAlive == false)
            return;

        IsAlive = false;
        _stateMachine.ChangeState(new HurtState(this, _stateMachine));
    }
}
