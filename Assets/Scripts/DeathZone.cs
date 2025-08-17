using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [Header("Death Zone Settings")]
    public bool instantDeath = true;
    public float damage = 100f;
    public string[] targetTags = { "Player" };

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsValidTarget(other.tag))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsValidTarget(collision.gameObject.tag))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }
        }
    }

    bool IsValidTarget(string tag)
    {
        foreach (string targetTag in targetTags)
        {
            if (tag == targetTag)
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        // Визуализация в редакторе
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = Color.red;
            if (col is BoxCollider2D)
            {
                BoxCollider2D boxCol = (BoxCollider2D)col;
                Gizmos.DrawWireCube(transform.position + (Vector3)boxCol.offset, boxCol.size);
            }
            else if (col is CircleCollider2D)
            {
                CircleCollider2D circleCol = (CircleCollider2D)col;
                Gizmos.DrawWireSphere(transform.position + (Vector3)circleCol.offset, circleCol.radius);
            }
        }
    }
}