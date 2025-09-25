using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : CharacterMovement
{
    public void Stop()
    {
        _rigidbody.linearVelocity = new Vector2(0, _rigidbody.linearVelocity.y);
    }
}
