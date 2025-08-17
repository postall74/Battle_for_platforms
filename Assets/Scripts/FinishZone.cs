using UnityEngine;

public class FinishZone : MonoBehaviour
{
    [Header("Finish Zone Settings")]
    public string[] targetTags = { "Player" };
    public bool requiresAllGhosts = false; // Нужно ли, чтобы все призраки тоже достигли финиша

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsValidTarget(other.tag))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                CompleteLevel();
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
                CompleteLevel();
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

    void CompleteLevel()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.Victory();
        }
        
        Debug.Log("Level Complete!");
    }

    void OnDrawGizmos()
    {
        // Визуализация в редакторе
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = Color.green;
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