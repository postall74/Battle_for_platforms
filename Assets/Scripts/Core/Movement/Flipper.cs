using System;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    private bool _startFacingPosition;
    private float _rightAngleRotate = 0f;
    private float _leftAngleRotate = 180f;

    public event Action<bool> Flipped;
    public bool IsFacingRight { get; private set; } = true;

    private void Start()
    {
        _startFacingPosition = IsFacingRight;
    }

    public void Flip(float direction)
    {
        if (direction == 0)
            return;

        bool shouldFaceRight = direction > 0;

        if (shouldFaceRight == IsFacingRight)
            return;

        IsFacingRight = shouldFaceRight;

        if (_startFacingPosition)
            transform.rotation = Quaternion.Euler(0f, shouldFaceRight ? _rightAngleRotate : _leftAngleRotate, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, shouldFaceRight ? _leftAngleRotate : _rightAngleRotate, 0f);

        Flipped?.Invoke(IsFacingRight);
    }
}