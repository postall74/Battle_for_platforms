using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    public Transform playerSpawnPoint;
    public Transform finishPoint;
    
    [Header("UI")]
    public Text deathCountText;
    public Text instructionText;
    public GameObject levelCompletePanel;
    public Button restartButton;
    public Button nextLevelButton;
    
    [Header("Game Settings")]
    public int deathCount = 0;
    public bool levelCompleted = false;
    
    private PlayerController player;
    private CameraFollow cameraFollow;
    
    void Start()
    {
        // Find player
        player = FindObjectOfType<PlayerController>();
        if (player != null && playerSpawnPoint != null)
        {
            player.transform.position = playerSpawnPoint.position;
        }
        
        // Setup camera
        cameraFollow = FindObjectOfType<CameraFollow>();
        if (cameraFollow != null && player != null)
        {
            cameraFollow.SetTarget(player.transform);
        }
        
        // Setup UI
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }
        
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartLevel);
        }
        
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(NextLevel);
        }
        
        UpdateUI();
        ShowInstructions();
    }
    
    void Update()
    {
        // Check for level completion
        if (!levelCompleted && player != null && finishPoint != null)
        {
            float distanceToFinish = Vector2.Distance(player.transform.position, finishPoint.position);
            if (distanceToFinish < 1f)
            {
                CompleteLevel();
            }
        }
        
        // Check for restart input
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }
    
    public void PlayerDied()
    {
        deathCount++;
        UpdateUI();
    }
    
    void CompleteLevel()
    {
        levelCompleted = true;
        
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }
        
        Debug.Log("Level Complete! Deaths: " + deathCount);
    }
    
    void UpdateUI()
    {
        if (deathCountText != null)
        {
            deathCountText.text = "Deaths: " + deathCount;
        }
    }
    
    void ShowInstructions()
    {
        if (instructionText != null)
        {
            instructionText.text = "WASD/Arrows: Move\nSpace: Jump\nR: Restart\n\nWhen you die, your ghost will help you!";
            
            // Hide instructions after 5 seconds
            StartCoroutine(HideInstructions());
        }
    }
    
    IEnumerator HideInstructions()
    {
        yield return new WaitForSeconds(5f);
        
        if (instructionText != null)
        {
            instructionText.text = "";
        }
    }
    
    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void NextLevel()
    {
        // Load next level or main menu
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0); // Load first scene
        }
    }
}