using System;

public class Health : TankProperty, ITakeDamageFromPlayer
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
                _current = 0;
                ZeroHealth?.Invoke();
            }
            RaiseValueChanged();
        }
    }

    public Health(int current) : base(current) { }

    public void TakeDamage(int damage) => Current -= damage;

    public void TakeDamage(int damage, string damagerName) => TakeDamage(damage);

    public void RestoreHealth(int amt)
    {
        Current += amt;
    }
}
