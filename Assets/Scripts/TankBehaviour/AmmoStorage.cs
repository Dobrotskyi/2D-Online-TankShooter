public class AmmoStorage
{
    private readonly int _maxAmmoAmt;
    private int _ammoLeft;
    public int AmmoLeft
    {
        get => _ammoLeft;
        private set
        {
            _ammoLeft = value;
            if (_ammoLeft > _maxAmmoAmt)
                _ammoLeft = _maxAmmoAmt;
            if (_ammoLeft < 0)
                _ammoLeft = 0;
        }
    }

    public AmmoStorage(int maxAmmoAmt)
    {
        _maxAmmoAmt = maxAmmoAmt;
        _ammoLeft = _maxAmmoAmt;
    }

    public void RessuplyAmmo(int amt) => AmmoLeft += amt;

    public bool LoadTurret()
    {
        if (AmmoLeft > 0)
        {
            AmmoLeft--;
            return true;
        }
        else
            return false;
    }

    public void LoadTurret(TurretPartBehav turret)
    {
        if (turret.Loaded || AmmoLeft == 0)
            return;

        if (AmmoLeft > 0)
        {
            turret.Loaded = true;
            AmmoLeft--;
        }
        else
            turret.Loaded = false;
    }
}
