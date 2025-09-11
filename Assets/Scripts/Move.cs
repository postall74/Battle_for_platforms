using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private float _moveInput;
    private Player _player;
    private Animation _animation;

    private void Awake()
    {
        TryGetComponent<Player>(out _player);
        TryGetComponent<Animation>(out _animation);
    }

    private void Update()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");
        Flip();
        _animation.PlayAnimationRun(Mathf.Abs(_moveInput));
    }

    private void FixedUpdate()
    {
        _player.Rigidbody.linearVelocity = new Vector2(_speed * _moveInput, _player.Rigidbody.linearVelocity.y);
    }

    private void Flip()
    {
        if (_moveInput != 0)
            _player.SpriteRenderer.flipX = _moveInput < 0;
    }
}
