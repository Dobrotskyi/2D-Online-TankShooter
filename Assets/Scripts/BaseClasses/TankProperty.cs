using System;
using UnityEngine;

public abstract class TankProperty
{
    public event Action ValueChanged;
    protected readonly int _max;
    protected int _current;

    public virtual int Current { get; protected set; }
    public float CurrentInPercents => (float)_current / _max;

    public TankProperty(int max)
    {
        _max = max;
        _current = _max;
    }

    protected void RaiseValueChanged() => ValueChanged?.Invoke();
}
