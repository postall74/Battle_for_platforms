using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("UI References")]
    public Text ghostCountText;
    public Text instructionText;
    public GameObject pausePanel;
    public Button restartButton;
    public Button quitButton;

    private GameManager gameManager;
    private bool isPaused = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        // Настройка кнопок
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }

        // Скрываем панель паузы
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Показываем инструкции
        ShowInstructions();
    }

    void Update()
    {
        UpdateGhostCount();
        
        // Обработка паузы
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void UpdateGhostCount()
    {
        if (ghostCountText != null && gameManager != null)
        {
            int ghostCount = gameManager.GetActiveGhosts().Count;
            ghostCountText.text = "Призраки: " + ghostCount;
        }
    }

    void ShowInstructions()
    {
        if (instructionText != null)
        {
            instructionText.text = "Управление:\n" +
                                  "WASD / Стрелки - Движение\n" +
                                  "Пробел - Прыжок\n" +
                                  "ESC - Пауза\n\n" +
                                  "Цель: Достичь финиша, используя призраков как платформы!";
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }

        Time.timeScale = isPaused ? 0f : 1f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        if (gameManager != null)
        {
            gameManager.RestartLevel();
        }
    }

    void QuitGame()
    {
        Time.timeScale = 1f;
        if (gameManager != null)
        {
            gameManager.QuitGame();
        }
    }

    public void ShowVictoryMessage()
    {
        if (instructionText != null)
        {
            instructionText.text = "Победа!\nУровень пройден!\nНажмите R для перезапуска";
        }
    }

    public void ShowGameOverMessage()
    {
        if (instructionText != null)
        {
            instructionText.text = "Игра окончена!\nНажмите R для перезапуска";
        }
    }
}