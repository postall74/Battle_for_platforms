using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyMovement : CharacterMovement
{
    public void Stop()
    {
        if(Rigidbody != null)
            Rigidbody.linearVelocity = new Vector2(0, Rigidbody.linearVelocity.y);
    }
}