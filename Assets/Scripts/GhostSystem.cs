using System.Collections.Generic;
using UnityEngine;

public class GhostSystem : MonoBehaviour
{
    [Header("Ghost Settings")]
    public GameObject ghostPrefab;
    public int maxGhosts = 5;
    public Color ghostColor = new Color(1f, 1f, 1f, 0.5f);
    
    private List<Ghost> activeGhosts = new List<Ghost>();
    private PlayerController player;
    
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.OnPlayerDied += CreateGhost;
        }
    }
    
    void CreateGhost(List<PlayerAction> recording)
    {
        if (recording.Count == 0) return;
        
        // Удаляем старых призраков если превышен лимит
        if (activeGhosts.Count >= maxGhosts)
        {
            RemoveOldestGhost();
        }
        
        // Создаем новый объект призрака
        GameObject ghostObj = Instantiate(ghostPrefab, recording[0].position, Quaternion.identity);
        Ghost ghost = ghostObj.GetComponent<Ghost>();
        
        if (ghost == null)
        {
            ghost = ghostObj.AddComponent<Ghost>();
        }
        
        // Настраиваем визуал призрака
        SpriteRenderer ghostRenderer = ghostObj.GetComponent<SpriteRenderer>();
        if (ghostRenderer != null)
        {
            ghostRenderer.color = ghostColor;
        }
        
        // Настраиваем коллайдер как триггер
        Collider2D ghostCollider = ghostObj.GetComponent<Collider2D>();
        if (ghostCollider != null)
        {
            ghostCollider.isTrigger = true;
        }
        
        // Добавляем тег
        ghostObj.tag = "Ghost";
        
        // Запускаем воспроизведение
        ghost.Initialize(recording);
        activeGhosts.Add(ghost);
    }
    
    void RemoveOldestGhost()
    {
        if (activeGhosts.Count > 0)
        {
            Ghost oldestGhost = activeGhosts[0];
            activeGhosts.RemoveAt(0);
            
            if (oldestGhost != null && oldestGhost.gameObject != null)
            {
                Destroy(oldestGhost.gameObject);
            }
        }
    }
    
    public void ClearAllGhosts()
    {
        foreach (var ghost in activeGhosts)
        {
            if (ghost != null && ghost.gameObject != null)
            {
                Destroy(ghost.gameObject);
            }
        }
        activeGhosts.Clear();
    }
}