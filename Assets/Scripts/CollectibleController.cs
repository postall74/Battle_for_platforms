using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private int _currentScore = 0;
    private List<ICollectible> _registeredCollectibles = new List<ICollectible>();

    public event Action<int> OnScoreChanged;

    public int CurrentScore => _currentScore;

    private void Start()
    {
        Coin[] coins = FindObjectsByType<Coin>(FindObjectsSortMode.None);

        foreach (Coin coin in coins)
            RegisterCollectible(coin);
    }

    public void RegisterCollectible(ICollectible collectible)
    {
        if (_registeredCollectibles.Contains(collectible) == false)
        {
            _registeredCollectibles.Add(collectible);
            collectible.OnCollected += HandleItemCollected;
        }
    }

    public void UnregisterCollectible(ICollectible collectible)
    {
        if (_registeredCollectibles.Contains(collectible))
        {
            _registeredCollectibles.Remove(collectible);
            collectible.OnCollected -= HandleItemCollected;
        }
    }

    private void HandleItemCollected(ICollectible collectible)
    {
        _currentScore += collectible.ScoreValue;
        OnScoreChanged?.Invoke(_currentScore);

        UnregisterCollectible(collectible);
    }
}