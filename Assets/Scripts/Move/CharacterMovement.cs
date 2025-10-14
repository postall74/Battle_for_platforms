using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Flipper), typeof(GroundChecker))]
public class CharacterMovement : MonoBehaviour, IMovable
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 15f;

    public event Action<float> Movement;
    public event Action Jumped;

    public Rigidbody2D Rigidbody { get; private set; }
    public Flipper Flipper { get; private set; }
    public GroundChecker GroundChecker { get; private set; }

    public bool IsGrounded => GroundChecker != null ? GroundChecker.IsGrounded : false;
    public float Speed => _speed;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Flipper = GetComponent<Flipper>();
        GroundChecker = GetComponent<GroundChecker>();
    }

    public void Move(float direction)
    {
        Flipper.Flip(direction);
        Rigidbody.linearVelocity = new Vector2(_speed * direction, Rigidbody.linearVelocity.y);
        Movement?.Invoke(direction);
    }

    public void Jump()
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
}