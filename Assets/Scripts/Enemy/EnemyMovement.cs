using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Flipper), typeof(GroundChecker))]
public class EnemyMovement : CharacterMovement
{
    public void Stop()
    {
        if (Rigidbody != null)
            Rigidbody.linearVelocity = new Vector2(0, Rigidbody.linearVelocity.y);
    }
}