using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField] private bool _isFacingRight = true;

    private readonly float _rightRotation = 0f;
    private readonly float _leftRotation = 180f;

    public bool IsFacingRight => _isFacingRight;

    private void Start()
    {
        ApplyInitialRotation();
    }

    public void HandleMovement(float direction)
    {
        if (direction == 0)
            return;

        bool shouldFaceRight = direction > 0;

        if (shouldFaceRight == _isFacingRight)
            return;

        _isFacingRight = shouldFaceRight;
        ApplyRotation();
    }

    private void ApplyInitialRotation()
    {
        ApplyRotation();
    }

    private void ApplyRotation()
    {
        float targetRotation = _isFacingRight ? _leftRotation : _rightRotation;
        transform.rotation = Quaternion.Euler(0f, targetRotation, 0f);
    }
}