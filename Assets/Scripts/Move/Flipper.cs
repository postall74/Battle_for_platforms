using System;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField] private bool _isFacingRight = true;
    private bool _startFacingPosition;
    private float _rightAngleRotate = 0f;
    private float _leftAngleRotate = 180f;

    public event Action<bool> Flipped;

    private void Start()
    {
        _startFacingPosition = _isFacingRight;
    }

    public bool IsFacingRight => _isFacingRight;

    public void Flip(float direction)
    {
        if (direction == 0)
            return;

        bool shouldFaceRight = direction > 0;

        if (shouldFaceRight == _isFacingRight)
            return;

        _isFacingRight = shouldFaceRight;

        if (_startFacingPosition)
            transform.rotation = Quaternion.Euler(0f, shouldFaceRight ? _rightAngleRotate : _leftAngleRotate, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, shouldFaceRight ? _leftAngleRotate : _rightAngleRotate, 0f);

        Flipped?.Invoke(_isFacingRight);
    }
}