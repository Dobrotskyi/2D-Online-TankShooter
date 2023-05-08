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

    public bool LoadTurret()
    {
        if (Current > 0)
        {
            Current--;
            return true;
        }
        else
            return false;
    }

    public void LoadTurret(TurretPartBehav turret)
    {
        if (turret.Loaded || Current == 0)
            return;

        if (Current > 0)
        {
            turret.Loaded = true;
            Current--;
        }
        else
            turret.Loaded = false;
    }
}
