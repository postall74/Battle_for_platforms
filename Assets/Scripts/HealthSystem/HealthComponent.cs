using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteBlinker))]
public class HealthComponent : MonoBehaviour, IDamageable, IHealable
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private bool _destroyOnDeath = false;
    [SerializeField] private float _deathEffectDuration = 1f;

    private HealthSystem _healthSystem;
    private SpriteBlinker _spriteBlinker;
    private Collider2D _collider;
    private List<MonoBehaviour> _disabledComponents = new List<MonoBehaviour>();

    public event Action<int> HealthChanged;
    public event Action<int> DamageTaken;
    public event Action<int> Healed;
    public event Action Died;

    public bool IsAlive => _healthSystem.IsAlive;
    public int CurrentHealth => _healthSystem.CurrentHealth;
    public int MaxHealth => _maxHealth;

    private void Awake()
    {
        _spriteBlinker = GetComponent<SpriteBlinker>();
        _collider = GetComponent<Collider2D>();

        _healthSystem = new HealthSystem(_maxHealth);
        _healthSystem.DamageTaken += OnDamageTaken;
        _healthSystem.Healed += OnHealed;
        _healthSystem.Died += OnDied;
        _healthSystem.HealthChanged += OnHealthChanged;
    }

    public void TakeDamage(int damage)
    {
        _healthSystem.TakeDamage(damage);
    }

    public void Heal(int amount)
    {
        _healthSystem.Heal(amount);
    }

    public void HandleDamageEffect()
    {
        _spriteBlinker?.BlinkDamage();
    }

    public void HandleHealEffect()
    {
        _spriteBlinker?.BlinkHeal();
    }

    private void OnDamageTaken(int damage)
    {
        HandleDamageEffect();
        DamageTaken?.Invoke(damage);
    }

    private void OnHealed(int amount)
    {
        HandleHealEffect();
        Healed?.Invoke(amount);
    }

    private void OnDied()
    {
        HandleDeathEffect();
        Died?.Invoke();
    }

    private void OnHealthChanged(int health)
    {
        HealthChanged?.Invoke(health);
    }

    private void HandleDeathEffect()
    {
        // Отключаем коллайдер
        if (_collider != null)
            _collider.enabled = false;

        // Запускаем эффект смерти
        _spriteBlinker?.StartDeathEffect();

        // Отключаем все MonoBehaviour компоненты, кроме необходимых
        DisableComponentsOnDeath();

        if (_destroyOnDeath)
            Destroy(gameObject, _deathEffectDuration);
        else
            StartCoroutine(DeactivateAfterDelay(_deathEffectDuration));
    }

    private void DisableComponentsOnDeath()
    {
        // Отключаем все MonoBehaviour компоненты, кроме этого и SpriteBlinker
        MonoBehaviour[] components = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour component in components)
        {
            // Пропускаем HealthComponent и SpriteBlinker
            if (component == this || component == _spriteBlinker)
                continue;

            // Пропускаем компоненты, которые уже отключены
            if (!component.enabled)
                continue;

            // Отключаем компонент и запоминаем его
            component.enabled = false;
            _disabledComponents.Add(component);
        }

        // Также отключаем Rigidbody2D, если есть
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.simulated = false;
    }

    private System.Collections.IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    public void Revive()
    {
        // Восстанавливаем здоровье
        _healthSystem = new HealthSystem(_maxHealth);

        // Включаем коллайдер
        if (_collider != null)
            _collider.enabled = true;

        // Сбрасываем цвет
        _spriteBlinker?.ResetColor();

        // Включаем все отключенные компоненты обратно
        EnableDisabledComponents();

        // Включаем Rigidbody2D, если есть
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.simulated = true;

        gameObject.SetActive(true);
    }

    private void EnableDisabledComponents()
    {
        foreach (MonoBehaviour component in _disabledComponents)
        {
            if (component != null)
                component.enabled = true;
        }

        _disabledComponents.Clear();
    }
}