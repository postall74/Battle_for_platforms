using System;
using System.Linq;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private int _currentScore;

    public event Action<int> OnScoreChanged;

    public int CurrentScore => _currentScore;

    private void OnEnable()
    {
        ICollectible[] collectibles = FindAnyObjectByType<MonoBehaviour>().GetComponents<ICollectible>().ToArray();

        foreach (var collectible in collectibles)
            collectible.OnCollected += HandleItemCollected;
    }

    private void OnDisable()
    {
        ICollectible[] collectibles = FindAnyObjectByType<MonoBehaviour>().GetComponents<ICollectible>().ToArray();

        foreach(var collectible in collectibles)
            collectible.OnCollected -= HandleItemCollected;
    }

    private void HandleItemCollected(ICollectible collectible)
    {
        _currentScore += collectible.ScoreValue;
        OnScoreChanged?.Invoke(_currentScore);
    }
}