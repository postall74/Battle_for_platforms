using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Spike Settings")]
    public bool isActive = true;
    public float damage = 100f;
    
    [Header("Visual")]
    public Color spikeColor = Color.red;
    
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = spikeColor;
        }
        
        // Set tag
        gameObject.tag = "DeathZone";
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;
        
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
    
    public void SetActive(bool active)
    {
        isActive = active;
        
        // Change color based on state
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isActive ? spikeColor : Color.gray;
        }
    }
}