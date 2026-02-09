using System;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private int _groundRaysCount = 3;
    [SerializeField] private float _groundRaysSpread = 0.2f;

    public event Action<bool> GroundedChanged;

    public bool IsGrounded { get; private set; }

    private void FixedUpdate()
    {
        bool wasGrounded = IsGrounded;
        CheckGrounded();

        if (wasGrounded != IsGrounded)
            GroundedChanged?.Invoke(IsGrounded);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_groundCheck == null)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < _groundRaysCount; i++)
        {
            float xOffset = -_groundRaysSpread + (i * _groundRaysSpread);
            Vector2 rayOrigin = _groundCheck.position + new Vector3(xOffset, 0, 0);
            Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.down * _groundCheckDistance);
        }
    }
#endif

    private void CheckGrounded()
    {
        IsGrounded = false;

        if (_groundCheck == null)
            return;

        for (int i = 0; i < _groundRaysCount; i++)
        {
            float xOffset = -_groundRaysSpread + (i * _groundRaysSpread);
            Vector2 rayOrigin = _groundCheck.position + new Vector3(xOffset, 0, 0);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, _groundCheckDistance, _groundLayer);

            if (hit.collider != null)
            {
                IsGrounded = true;
                break;
            }
        }
    }
}