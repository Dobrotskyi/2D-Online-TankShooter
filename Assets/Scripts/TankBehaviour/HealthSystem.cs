using System;
using UnityEngine;
public class HealthSystem : ITakeDamage
{
    public event Action ZeroHealth;
    private readonly int _maxHealth;
    private int _health;

    public int Health
    {
        get => _health;

        set
        {
            _health = value;
            if (_health > _maxHealth)
                _health = _maxHealth;
            if (_health <= 0)
            {
                ZeroHealth?.Invoke();
                Debug.Log("Dead");
            }
        }
    }

    public HealthSystem(int maxHealth)
    {
        _maxHealth = maxHealth;
        _health = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log(Health);
    }
}
