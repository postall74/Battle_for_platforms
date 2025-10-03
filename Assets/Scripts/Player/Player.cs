using System;
using UnityEngine;
using UnityEngine.Scripting;

[RequireComponent(typeof(PlayerAnimation), typeof(CharacterMovement), typeof(Collector))]
public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private InputReader _inputReader;
    private CharacterMovement _movement;
    private PlayerAnimation _animation;
    private Collector _collector;

    public Rigidbody2D Rigidbody => _rigidbody;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public InputReader InputReader => _inputReader;
    public CharacterMovement Movement => _movement;
    public PlayerAnimation Animation => _animation;
    public Collector Collector => _collector;

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
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _inputReader = GetComponent<InputReader>();
        _movement = GetComponent<CharacterMovement>();
        _animation = GetComponent<PlayerAnimation>();
        _collector = GetComponent<Collector>();
    }

    private void SubscribeToEvents()
    {
        _movement.GroundedChanged += _animation.HandleGroundedChanged;
        _movement.Movement += _animation.HandleMovement;
        _movement.Jumped += _animation.HandleJump;
    }

    private void UnsubscribeFromEvents()
    {
        if (_movement != null)
        {
            _movement.GroundedChanged -= _animation.HandleGroundedChanged;
            _movement.Movement -= _animation.HandleMovement;
            _movement.Jumped -= _animation.HandleJump;
        }
    }

    private void HandleMovement()
    {
        _movement.Move(_inputReader.HorizontalDirection);
        _animation.HandleVerticalVelocity(_movement.GetVerticalVelocity());

    }

    private void HandleInput()
    {
        if (_inputReader.WasJumpPressed)
            _movement.Jump();
    }
}