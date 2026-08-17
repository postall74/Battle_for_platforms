using NUnit.Framework;
using UnityEngine;

/// <summary>
/// Тесты для компонента HealthProvider.
/// Проверяют корректность работы системы здоровья: получение урона, лечение, смерть и неуязвимость.
/// </summary>
[TestFixture]
public class HealthProviderTests
{
    private GameObject _gameObject;
    private HealthProvider _healthProvider;

    [SetUp]
    public void Setup()
    {
        _gameObject = new GameObject("TestHealth");
        _healthProvider = _gameObject.AddComponent<HealthProvider>();
        // Инициализируем через Start
        _healthProvider.GetType().GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(_healthProvider, null);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_gameObject);
    }

    /// <summary>
    /// Проверка начального значения здоровья.
    /// </summary>
    [Test]
    public void InitialHealth_ShouldBeMaxHealth()
    {
        Assert.AreEqual(100, _healthProvider.CurrentHealth);
        Assert.AreEqual(100, _healthProvider.MaxHealth);
    }

    /// <summary>
    /// Проверка что объект жив при полном здоровье.
    /// </summary>
    [Test]
    public void IsAlive_WithFullHealth_ShouldBeTrue()
    {
        Assert.IsTrue(_healthProvider.IsAlive);
    }

    /// <summary>
    /// Проверка получения урона.
    /// </summary>
    [Test]
    public void TakeDamage_ShouldReduceHealth()
    {
        _healthProvider.TakeDamage(30);
        
        Assert.AreEqual(70, _healthProvider.CurrentHealth);
    }

    /// <summary>
    /// Проверка что здоровье не опускается ниже нуля.
    /// </summary>
    [Test]
    public void TakeDamage_WithLethalDamage_ShouldNotGoBelowZero()
    {
        _healthProvider.TakeDamage(150);
        
        Assert.AreEqual(0, _healthProvider.CurrentHealth);
    }

    /// <summary>
    /// Проверка события смерти.
    /// </summary>
    [Test]
    public void TakeDamage_WithLethalDamage_ShouldInvokeDiedEvent()
    {
        bool diedEventInvoked = false;
        _healthProvider.Died += () => diedEventInvoked = true;
        
        _healthProvider.TakeDamage(100);
        
        Assert.IsTrue(diedEventInvoked);
    }

    /// <summary>
    /// Проверка что после смерти объект не жив.
    /// </summary>
    [Test]
    public void IsAlive_AfterDeath_ShouldBeFalse()
    {
        _healthProvider.TakeDamage(100);
        
        Assert.IsFalse(_healthProvider.IsAlive);
    }

    /// <summary>
    /// Проверка лечения.
    /// </summary>
    [Test]
    public void Heal_ShouldIncreaseHealth()
    {
        _healthProvider.TakeDamage(50);
        _healthProvider.Heal(20);
        
        Assert.AreEqual(70, _healthProvider.CurrentHealth);
    }

    /// <summary>
    /// Проверка что лечение не превышает максимальное здоровье.
    /// </summary>
    [Test]
    public void Heal_WithExcessiveAmount_ShouldNotExceedMaxHealth()
    {
        _healthProvider.TakeDamage(30);
        _healthProvider.Heal(100);
        
        Assert.AreEqual(100, _healthProvider.CurrentHealth);
    }

    /// <summary>
    /// Проверка что мертвый объект нельзя лечить.
    /// </summary>
    [Test]
    public void Heal_OnDeadObject_ShouldDoNothing()
    {
        _healthProvider.TakeDamage(100);
        int healthBeforeHeal = _healthProvider.CurrentHealth;
        
        _healthProvider.Heal(50);
        
        Assert.AreEqual(healthBeforeHeal, _healthProvider.CurrentHealth);
    }

    /// <summary>
    /// Проверка неуязвимости после получения урона.
    /// </summary>
    [Test]
    public void TakeDamage_DuringInvincibility_ShouldDoNothing()
    {
        _healthProvider.TakeDamage(30);
        int healthAfterFirstDamage = _healthProvider.CurrentHealth;
        
        // Сразу получаем еще урон (должен быть проигнорирован из-за неуязвимости)
        _healthProvider.TakeDamage(30);
        
        Assert.AreEqual(healthAfterFirstDamage, _healthProvider.CurrentHealth);
    }

    /// <summary>
    /// Проверка события изменения здоровья.
    /// </summary>
    [Test]
    public void TakeDamage_ShouldInvokeHealthChangedEvent()
    {
        int receivedCurrentHealth = -1;
        int receivedMaxHealth = -1;
        _healthProvider.HealthChanged += (current, max) => 
        {
            receivedCurrentHealth = current;
            receivedMaxHealth = max;
        };
        
        _healthProvider.TakeDamage(30);
        
        Assert.AreEqual(70, receivedCurrentHealth);
        Assert.AreEqual(100, receivedMaxHealth);
    }
}
