public class AmmoStorage : TankProperty
{
    public override int Current
    {
        get => _current;

        protected set
        {
            _current = value;
            if (_current > _max)
                _current = _max;
            if (_current < 0)
                _current = 0;
        }
    }

    public AmmoStorage(int maxAmmoAmt) : base(maxAmmoAmt) { }

    public void RessuplyAmmo(int amt) => Current += amt;

    public bool GiveAmmo(int amt)
    {
        if (Current >= amt)
        {
            Current -= amt;
            return true;
        }
        return false;
    }
}
