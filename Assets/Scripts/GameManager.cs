using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public Transform spawnPoint;
    public GameObject ghostPrefab;
    public int maxGhosts = 5;
    public float ghostLifetime = 30f; // Время жизни призрака в секундах

    [Header("UI References")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    // Список активных призраков
    private List<Ghost> activeGhosts = new List<Ghost>();
    private PlayerController player;

    // Состояние игры
    private bool gameWon = false;
    private bool gameOver = false;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        
        // Скрываем UI панели
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    void Update()
    {
        // Очистка неактивных призраков
        CleanupInactiveGhosts();

        // Проверка победы
        CheckVictory();
    }

    public void CreateGhost(List<PlayerController.PlayerAction> recordedActions)
    {
        if (ghostPrefab == null || recordedActions == null || recordedActions.Count == 0)
        {
            Debug.LogWarning("Cannot create ghost: missing prefab or no recorded actions");
            return;
        }

        // Ограничиваем количество призраков
        if (activeGhosts.Count >= maxGhosts)
        {
            RemoveOldestGhost();
        }

        // Создаем призрака
        GameObject ghostObject = Instantiate(ghostPrefab, spawnPoint.position, Quaternion.identity);
        Ghost ghost = ghostObject.GetComponent<Ghost>();

        if (ghost != null)
        {
            ghost.StartPlayback(recordedActions);
            activeGhosts.Add(ghost);

            // Устанавливаем время жизни призрака
            StartCoroutine(DestroyGhostAfterTime(ghost, ghostLifetime));
        }
    }

    void RemoveOldestGhost()
    {
        if (activeGhosts.Count > 0)
        {
            Ghost oldestGhost = activeGhosts[0];
            activeGhosts.RemoveAt(0);
            
            if (oldestGhost != null)
            {
                oldestGhost.DestroyGhost();
            }
        }
    }

    void CleanupInactiveGhosts()
    {
        for (int i = activeGhosts.Count - 1; i >= 0; i--)
        {
            if (activeGhosts[i] == null || !activeGhosts[i].IsPlaying())
            {
                activeGhosts.RemoveAt(i);
            }
        }
    }

    IEnumerator DestroyGhostAfterTime(Ghost ghost, float time)
    {
        yield return new WaitForSeconds(time);
        
        if (ghost != null && activeGhosts.Contains(ghost))
        {
            activeGhosts.Remove(ghost);
            ghost.DestroyGhost();
        }
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint != null ? spawnPoint.position : Vector3.zero;
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }

    public void GameOver()
    {
        if (gameOver) return;

        gameOver = true;
        Debug.Log("Game Over!");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void Victory()
    {
        if (gameWon) return;

        gameWon = true;
        Debug.Log("Victory!");

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }

    void CheckVictory()
    {
        // Здесь можно добавить логику проверки победы
        // Например, если игрок достиг определенной точки
    }

    public void RestartLevel()
    {
        // Перезагружаем сцену
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public List<Ghost> GetActiveGhosts()
    {
        return new List<Ghost>(activeGhosts);
    }

    public void ClearAllGhosts()
    {
        foreach (Ghost ghost in activeGhosts)
        {
            if (ghost != null)
            {
                ghost.DestroyGhost();
            }
        }
        activeGhosts.Clear();
    }
}