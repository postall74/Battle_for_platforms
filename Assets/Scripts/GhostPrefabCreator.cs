using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPrefabCreator : MonoBehaviour
{
    [ContextMenu("Create Ghost Prefab")]
    public void CreateGhostPrefab()
    {
        // Create ghost GameObject
        GameObject ghost = new GameObject("Ghost");
        ghost.tag = "Ghost";
        
        // Add components
        SpriteRenderer ghostSprite = ghost.AddComponent<SpriteRenderer>();
        ghostSprite.sprite = SpriteGenerator.CreateCircleSprite(new Color(0.5f, 0.5f, 1f, 0.6f), 32);
        ghostSprite.color = new Color(0.5f, 0.5f, 1f, 0.6f);
        
        BoxCollider2D ghostCollider = ghost.AddComponent<BoxCollider2D>();
        ghostCollider.size = new Vector2(1f, 1f);
        
        Rigidbody2D ghostRb = ghost.AddComponent<Rigidbody2D>();
        ghostRb.isKinematic = true;
        
        ghost.AddComponent<GhostController>();
        
        // Create prefab
        #if UNITY_EDITOR
        string prefabPath = "Assets/Prefabs/Ghost.prefab";
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(ghost, prefabPath);
        Debug.Log("Ghost prefab created at: " + prefabPath);
        #endif
        
        // Destroy the temporary GameObject
        DestroyImmediate(ghost);
    }
}