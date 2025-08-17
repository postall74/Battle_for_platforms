using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    [Header("Setup Settings")]
    public bool autoSetup = true;
    public bool createPlayer = true;
    public bool createLevel = true;
    public bool createUI = true;
    
    void Start()
    {
        if (autoSetup)
        {
            SetupLevel();
        }
    }
    
    [ContextMenu("Setup Level")]
    public void SetupLevel()
    {
        if (createPlayer)
        {
            CreatePlayer();
        }
        
        if (createLevel)
        {
            CreateLevel();
        }
        
        if (createUI)
        {
            CreateUI();
        }
        
        SetupCamera();
        SetupLevelManager();
    }
    
    void CreatePlayer()
    {
        // Create Player
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        
        // Add components
        SpriteRenderer playerSprite = player.AddComponent<SpriteRenderer>();
        playerSprite.sprite = SpriteGenerator.CreateCircleSprite(Color.white, 32);
        
        Rigidbody2D playerRb = player.AddComponent<Rigidbody2D>();
        playerRb.gravityScale = 1f;
        
        BoxCollider2D playerCollider = player.AddComponent<BoxCollider2D>();
        playerCollider.size = new Vector2(1f, 1f);
        
        PlayerController playerController = player.AddComponent<PlayerController>();
        
        // Create GroundCheck
        GameObject groundCheck = new GameObject("GroundCheck");
        groundCheck.transform.SetParent(player.transform);
        groundCheck.transform.localPosition = new Vector3(0, -0.5f, 0);
        
        playerController.groundCheck = groundCheck.transform;
        playerController.groundLayer = LayerMask.GetMask("Ground");
        
        // Set position
        player.transform.position = new Vector3(-8, 2, 0);
    }
    
    void CreateLevel()
    {
        // Create ground platforms
        CreateGroundPlatform(new Vector3(0, -2, 0), new Vector3(20, 1, 1));
        CreateGroundPlatform(new Vector3(8, 0, 0), new Vector3(4, 1, 1));
        CreateGroundPlatform(new Vector3(15, 2, 0), new Vector3(4, 1, 1));
        
        // Create spikes
        CreateSpikes(new Vector3(2, -1, 0));
        CreateSpikes(new Vector3(4, -1, 0));
        CreateSpikes(new Vector3(6, -1, 0));
        
        // Create button and door
        GameObject button = CreateButton(new Vector3(10, 1, 0));
        GameObject door = CreateDoor(new Vector3(12, 0, 0));
        
        ButtonController buttonController = button.GetComponent<ButtonController>();
        buttonController.targetObject = door;
        
        // Create falling rock
        CreateFallingRock(new Vector3(5, 4, 0));
        
        // Create finish
        CreateFinish(new Vector3(18, 3, 0));
    }
    
    void CreateGroundPlatform(Vector3 position, Vector3 scale)
    {
        GameObject platform = new GameObject("Ground");
        platform.layer = LayerMask.NameToLayer("Ground");
        platform.tag = "Ground";
        
        SpriteRenderer sprite = platform.AddComponent<SpriteRenderer>();
        sprite.sprite = SpriteGenerator.CreateSquareSprite(Color.green, 32);
        sprite.color = Color.green;
        
        BoxCollider2D collider = platform.AddComponent<BoxCollider2D>();
        
        platform.transform.position = position;
        platform.transform.localScale = scale;
    }
    
    void CreateSpikes(Vector3 position)
    {
        GameObject spikes = new GameObject("Spikes");
        spikes.tag = "DeathZone";
        
        SpriteRenderer sprite = spikes.AddComponent<SpriteRenderer>();
        sprite.sprite = SpriteGenerator.CreateTriangleSprite(Color.red, 32);
        sprite.color = Color.red;
        
        BoxCollider2D collider = spikes.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        
        spikes.AddComponent<SpikeTrap>();
        spikes.transform.position = position;
    }
    
    GameObject CreateButton(Vector3 position)
    {
        GameObject button = new GameObject("Button");
        button.tag = "Button";
        
        SpriteRenderer sprite = button.AddComponent<SpriteRenderer>();
        sprite.sprite = SpriteGenerator.CreateCircleSprite(Color.red, 24);
        sprite.color = Color.red;
        
        BoxCollider2D collider = button.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        
        button.AddComponent<ButtonController>();
        button.transform.position = position;
        
        return button;
    }
    
    GameObject CreateDoor(Vector3 position)
    {
        GameObject door = new GameObject("Door");
        door.tag = "Door";
        
        SpriteRenderer sprite = door.AddComponent<SpriteRenderer>();
        sprite.sprite = SpriteGenerator.CreateSquareSprite(Color.brown, 32);
        sprite.color = Color.brown;
        
        BoxCollider2D collider = door.AddComponent<BoxCollider2D>();
        
        door.AddComponent<DoorController>();
        door.transform.position = position;
        
        return door;
    }
    
    void CreateFallingRock(Vector3 position)
    {
        GameObject rock = new GameObject("FallingRock");
        rock.tag = "DeathZone";
        
        SpriteRenderer sprite = rock.AddComponent<SpriteRenderer>();
        sprite.sprite = SpriteGenerator.CreateCircleSprite(Color.gray, 32);
        sprite.color = Color.gray;
        
        Rigidbody2D rb = rock.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;
        
        BoxCollider2D collider = rock.AddComponent<BoxCollider2D>();
        
        rock.AddComponent<FallingRock>();
        rock.transform.position = position;
    }
    
    void CreateFinish(Vector3 position)
    {
        GameObject finish = new GameObject("Finish");
        finish.tag = "Finish";
        
        SpriteRenderer sprite = finish.AddComponent<SpriteRenderer>();
        sprite.sprite = SpriteGenerator.CreateSquareSprite(Color.yellow, 32);
        sprite.color = Color.yellow;
        
        BoxCollider2D collider = finish.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        
        finish.transform.position = position;
    }
    
    void CreateUI()
    {
        // Create Canvas
        GameObject canvas = new GameObject("Canvas");
        Canvas canvasComponent = canvas.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        // Create Death Count Text
        GameObject deathText = new GameObject("DeathCountText");
        deathText.transform.SetParent(canvas.transform);
        
        UnityEngine.UI.Text deathTextComponent = deathText.AddComponent<UnityEngine.UI.Text>();
        deathTextComponent.text = "Deaths: 0";
        deathTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        deathTextComponent.fontSize = 24;
        deathTextComponent.color = Color.white;
        
        RectTransform deathRect = deathText.GetComponent<RectTransform>();
        deathRect.anchorMin = new Vector2(0, 1);
        deathRect.anchorMax = new Vector2(0, 1);
        deathRect.anchoredPosition = new Vector2(100, -30);
        deathRect.sizeDelta = new Vector2(200, 50);
        
        // Create Instruction Text
        GameObject instructionText = new GameObject("InstructionText");
        instructionText.transform.SetParent(canvas.transform);
        
        UnityEngine.UI.Text instructionTextComponent = instructionText.AddComponent<UnityEngine.UI.Text>();
        instructionTextComponent.text = "WASD/Arrows: Move\nSpace: Jump\nR: Restart";
        instructionTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        instructionTextComponent.fontSize = 18;
        instructionTextComponent.color = Color.white;
        instructionTextComponent.alignment = TextAnchor.MiddleCenter;
        
        RectTransform instructionRect = instructionText.GetComponent<RectTransform>();
        instructionRect.anchorMin = new Vector2(0.5f, 0.5f);
        instructionRect.anchorMax = new Vector2(0.5f, 0.5f);
        instructionRect.anchoredPosition = new Vector2(0, 100);
        instructionRect.sizeDelta = new Vector2(400, 100);
        
        // Create Level Complete Panel
        GameObject completePanel = new GameObject("LevelCompletePanel");
        completePanel.transform.SetParent(canvas.transform);
        
        UnityEngine.UI.Image panelImage = completePanel.AddComponent<UnityEngine.UI.Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);
        
        RectTransform panelRect = completePanel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        
        // Create Level Complete Text
        GameObject completeText = new GameObject("CompleteText");
        completeText.transform.SetParent(completePanel.transform);
        
        UnityEngine.UI.Text completeTextComponent = completeText.AddComponent<UnityEngine.UI.Text>();
        completeTextComponent.text = "Level Complete!\nDeaths: 0";
        completeTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        completeTextComponent.fontSize = 36;
        completeTextComponent.color = Color.white;
        completeTextComponent.alignment = TextAnchor.MiddleCenter;
        
        RectTransform completeTextRect = completeText.GetComponent<RectTransform>();
        completeTextRect.anchorMin = new Vector2(0.5f, 0.5f);
        completeTextRect.anchorMax = new Vector2(0.5f, 0.5f);
        completeTextRect.anchoredPosition = new Vector2(0, 50);
        completeTextRect.sizeDelta = new Vector2(400, 100);
        
        // Create Restart Button
        GameObject restartButton = new GameObject("RestartButton");
        restartButton.transform.SetParent(completePanel.transform);
        
        UnityEngine.UI.Button restartButtonComponent = restartButton.AddComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Image restartButtonImage = restartButton.AddComponent<UnityEngine.UI.Image>();
        restartButtonImage.color = Color.blue;
        
        UnityEngine.UI.Text restartButtonText = restartButton.AddComponent<UnityEngine.UI.Text>();
        restartButtonText.text = "Restart";
        restartButtonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        restartButtonText.fontSize = 24;
        restartButtonText.color = Color.white;
        restartButtonText.alignment = TextAnchor.MiddleCenter;
        
        RectTransform restartButtonRect = restartButton.GetComponent<RectTransform>();
        restartButtonRect.anchorMin = new Vector2(0.5f, 0.5f);
        restartButtonRect.anchorMax = new Vector2(0.5f, 0.5f);
        restartButtonRect.anchoredPosition = new Vector2(-100, -50);
        restartButtonRect.sizeDelta = new Vector2(150, 50);
        
        completePanel.SetActive(false);
    }
    
    void SetupCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            CameraFollow cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                cameraFollow.target = player.transform;
            }
        }
    }
    
    void SetupLevelManager()
    {
        GameObject levelManager = new GameObject("LevelManager");
        LevelManager manager = levelManager.AddComponent<LevelManager>();
        
        // Find UI elements
        GameObject deathText = GameObject.Find("DeathCountText");
        GameObject instructionText = GameObject.Find("InstructionText");
        GameObject completePanel = GameObject.Find("LevelCompletePanel");
        GameObject restartButton = GameObject.Find("RestartButton");
        
        if (deathText != null)
            manager.deathCountText = deathText.GetComponent<UnityEngine.UI.Text>();
        if (instructionText != null)
            manager.instructionText = instructionText.GetComponent<UnityEngine.UI.Text>();
        if (completePanel != null)
            manager.levelCompletePanel = completePanel;
        if (restartButton != null)
            manager.restartButton = restartButton.GetComponent<UnityEngine.UI.Button>();
        
        // Set spawn and finish points
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject finish = GameObject.FindGameObjectWithTag("Finish");
        
        if (player != null)
        {
            GameObject spawnPoint = new GameObject("SpawnPoint");
            spawnPoint.transform.position = player.transform.position;
            manager.playerSpawnPoint = spawnPoint.transform;
        }
        
        if (finish != null)
        {
            manager.finishPoint = finish.transform;
        }
    }
}