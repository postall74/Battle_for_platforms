using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speedX = 5f;

    private Rigidbody2D _rigidbody;
    private Quaternion _orginalRotation;
    private readonly float _flipAngle = 180f;
    private Quaternion _flipRotation;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _orginalRotation = transform.rotation;
        _flipRotation = _orginalRotation * Quaternion.Euler(0, _flipAngle, 0);
    }

    public void Move(float direction)
    {
        Flip(direction);
        _rigidbody.linearVelocity = new Vector2(_speedX * direction, _rigidbody.linearVelocity.y);
    }

    private void Flip(float direction)
    {
        if (direction != 0)
            transform.rotation = direction < 0 ? _flipRotation : _orginalRotation;
    }
}
