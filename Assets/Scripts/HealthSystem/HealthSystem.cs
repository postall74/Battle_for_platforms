using System;
using UnityEngine;

public class HealthSystem
{
    private int _currentHealth;
    private int _maxHealth;

    public event Action<int> HealthChanged;
    public event Action<int> DamageTaken;
    public event Action<int> Healed;
    public event Action Died;
    
    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    public bool IsAlive => _currentHealth > 0;

    public HealthSystem(int maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int damage) 
    {
        if (IsAlive == false)
            return;

        _currentHealth = Math.Max(0, _currentHealth - damage);
        DamageTaken?.Invoke(damage);
        HealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
            Died?.Invoke();
    }

    public void Heal(int amount)
    {
        if (IsAlive == false) 
            return;

        int oldHealth = _currentHealth;
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
        int actualHeal = _currentHealth - oldHealth;

        if(actualHeal > 0)
        {
            Healed?.Invoke(actualHeal);
            HealthChanged?.Invoke(_currentHealth);
        }
    }
}