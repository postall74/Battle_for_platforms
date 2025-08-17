using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    public Color trapColor = Color.red;
    
    void Start()
    {
        // Устанавливаем тег
        gameObject.tag = "DeathTrap";
        
        // Настраиваем визуал
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = trapColor;
        }
        
        // Убеждаемся что коллайдер настроен как триггер
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }
}