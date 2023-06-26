public class AmmoStorage : TankProperty
{
    private int _lastShotPrice = -1;
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
            if (_current < _lastShotPrice)
                AmmoNotEnough = true;
            else
                AmmoNotEnough = false;

            RaiseValueChanged();
        }
    }

    public bool AmmoNotEnough { get; private set; } = false;

    public AmmoStorage(int maxAmmoAmt) : base(maxAmmoAmt) { }

    public void RessuplyAmmo(int amt) => Current += amt;

    public bool GiveAmmo(int amt)
    {
        if (Current >= amt)
        {
            Current -= amt;
            if (_lastShotPrice != amt)
                _lastShotPrice = amt;
            return true;
        }
        return false;
    }
}
