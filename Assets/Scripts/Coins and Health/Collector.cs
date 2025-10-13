using System;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private int _currentScore = 0;

    public event Action<int> ScoreChanged;
    public int CurrentScore => _currentScore;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<ICollectible>(out var collectible))
            Collect(collectible);
    }

    private void Collect(ICollectible collectible)
    {
        _currentScore++;
        ScoreChanged?.Invoke(_currentScore);
        collectible.Collect();

#if UNITY_EDITOR
        Debug.Log($"Collected item! Total score: {_currentScore}");
#endif
    }
}
