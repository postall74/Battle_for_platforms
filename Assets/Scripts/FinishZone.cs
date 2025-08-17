using UnityEngine;

public class FinishZone : MonoBehaviour
{
    [Header("Finish Settings")]
    public Color finishColor = Color.green;
    public bool showCompletionMessage = true;
    
    private bool levelCompleted = false;
    
    void Start()
    {
        // Устанавливаем тег
        gameObject.tag = "Finish";
        
        // Настраиваем визуал
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = finishColor;
        }
        
        // Убеждаемся что коллайдер настроен как триггер
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !levelCompleted)
        {
            levelCompleted = true;
            
            if (showCompletionMessage)
            {
                Debug.Log("Уровень пройден! Поздравляем!");
            }
            
            // Уведомляем менеджер уровня
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.CompleteLevel();
            }
        }
    }
}