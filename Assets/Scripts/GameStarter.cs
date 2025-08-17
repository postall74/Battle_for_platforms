using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [Header("Quick Start")]
    public bool startOnPlay = true;
    
    void Start()
    {
        if (startOnPlay)
        {
            StartGame();
        }
    }
    
    [ContextMenu("Start Game")]
    public void StartGame()
    {
        // Create layer if it doesn't exist
        CreateLayerIfNeeded("Ground");
        
        // Setup level
        LevelSetup levelSetup = gameObject.AddComponent<LevelSetup>();
        levelSetup.autoSetup = false;
        levelSetup.SetupLevel();
        
        Debug.Log("Game started! Use WASD/Arrows to move, Space to jump, R to restart.");
        Debug.Log("When you die, your ghost will help you complete the level!");
    }
    
    void CreateLayerIfNeeded(string layerName)
    {
        // Check if layer exists
        int layerIndex = LayerMask.NameToLayer(layerName);
        if (layerIndex == -1)
        {
            Debug.LogWarning($"Layer '{layerName}' doesn't exist. Please create it in Edit > Project Settings > Tags and Layers");
        }
    }
}