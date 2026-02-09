using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthCollector : MonoBehaviour
{
    private HealthComponent _healthComponent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHealthCollectible>(out var healthCollectible))
        {
            CollectHealth(healthCollectible);
        }
    }

    public void Initialize(HealthComponent healthComponent)
    {
        _healthComponent = healthComponent;
    }

    private void CollectHealth(IHealthCollectible healthCollectible)
    {
        _healthComponent.Heal(healthCollectible.HealAmount);
        healthCollectible.Collect();
    }
}