public abstract class TankProperty
{
    protected readonly int _max;
    protected int _current;

    public virtual int Current { get; protected set; }

    public TankProperty(int max)
    {
        _max = max;
        _current = _max;
    }
}
