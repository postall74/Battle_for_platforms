using UnityEngine;

public class AlarmTrigger : MonoBehaviour
{
    public event System.Action PlayerEntered;
    public event System.Action PlayerExited;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out _))
            PlayerEntered?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out _))
            PlayerExited?.Invoke();
    }
}