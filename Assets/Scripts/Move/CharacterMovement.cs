using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(GroundChecker), typeof(Flipper))]
public class CharacterMovement : MonoBehaviour, IMovable
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 15f;

    private GroundChecker _groundChecker;
    private Flipper _flipper;

    public event Action<float> Movement;
    public event Action Jumped;
    public event Action<bool> GroundedChanged;

    public Rigidbody2D Rigidbody { get; private set; }
    public bool IsGrounded => _groundChecker.IsGrounded;
    public float Speed => _speed;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _groundChecker = GetComponent<GroundChecker>();
        _flipper = GetComponent<Flipper>();

        _groundChecker.GroundedChanged += OnGroundedChanged;
    }

    private void OnDestroy()
    {
        if (_groundChecker != null)
            _groundChecker.GroundedChanged -= OnGroundedChanged;
    }

    public void Move(float direction)
    {
        Rigidbody.linearVelocity = new Vector2(_speed * direction, Rigidbody.linearVelocity.y);
        Movement?.Invoke(direction);
    }

    public  void Jump()
    {
        if (IsGrounded == false)
            return;

        Rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        Jumped?.Invoke();
    }

    public float GetVerticalVelocity()
    {
        return Rigidbody != null ? Rigidbody.linearVelocity.y : 0f;
    }

    private void OnGroundedChanged(bool isGrounded)
    {
        GroundedChanged?.Invoke(isGrounded);
    }
}