using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    public string nextLevelName = "";
    public bool enableRestartKey = true;
    
    private static LevelManager instance;
    private PlayerController player;
    private GhostSystem ghostSystem;
    
    public static LevelManager Instance
    {
        get { return instance; }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        ghostSystem = FindObjectOfType<GhostSystem>();
    }
    
    void Update()
    {
        HandleInput();
    }
    
    void HandleInput()
    {
        // Перезапуск уровня на R
        if (enableRestartKey && Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        
        // Выход из игры на Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }
    
    public void RestartLevel()
    {
        // Очищаем всех призраков
        if (ghostSystem != null)
        {
            ghostSystem.ClearAllGhosts();
        }
        
        // Перезагружаем сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.Log("Next level not set!");
        }
    }
    
    public void CompleteLevel()
    {
        Debug.Log("Level Completed!");
        
        // Здесь можно добавить эффекты завершения уровня
        // Например, показать экран победы или перейти к следующему уровню
        
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            Invoke("LoadNextLevel", 2f); // Задержка перед загрузкой следующего уровня
        }
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    // Вызывается когда игрок достигает финиша
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CompleteLevel();
        }
    }
}