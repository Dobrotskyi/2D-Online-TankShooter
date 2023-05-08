using System;
using UnityEngine;
public class Health : TankProperty, ITakeDamage
{
    public event Action ZeroHealth;

    public override int Current
    {
        get => _current;

        protected set
        {
            _current = value;
            if (_current > _max)
                _current = _max;
            if (_current <= 0)
            {
                ZeroHealth?.Invoke();
                Debug.Log("Dead");
            }
        }
    }

    public Health(int current) : base(current) { }

    public void TakeDamage(int damage)
    {
        Current -= damage;
        Debug.Log(Current);
    }
}
