using System;

public interface ICollectible
{
    public event Action<ICollectible> OnCollected;

    public int ScoreValue { get; }
}