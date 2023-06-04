using UnityEngine;

public class TurretPartBehav : MonoBehaviour
{
    private const float MIN_AIMING_DIST = 1f;

    public bool Loaded = false;

    private TurretData _turretData;
    private Transform _barrel;
    private Transform _mainPart;
    private float _fireRateMultiplier = 1f;
    private float _lastShotTime = 0f;

    public void SetData(TurretData data)
    {
        if (_turretData == null)
        {
            _turretData = data;
        }
        else
            throw new System.Exception("Data for this turret was already set");
    }

    public void AttachToBase(Transform mainPart)
    {
        if (_mainPart == null)
            _mainPart = mainPart;
        else
            throw new System.Exception("this turret already has a base");
    }

    public void AimAtTarget(Vector2 target)
    {
        Vector2 substraction = target - (Vector2)_barrel.transform.position;
        if (substraction.magnitude < MIN_AIMING_DIST)
            return;

        Vector2 lookDirection = (target - (Vector2)_barrel.transform.position);
        float angleZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angleZ, _barrel.transform.forward);
        gameObject.transform.rotation = Quaternion.Slerp(_barrel.transform.rotation, rotation, _turretData.RotationSpeed * Time.deltaTime);
    }

    public void Shoot()
    {
        if (CanShoot)
        {
            Vector2 direction = (Vector2)_barrel.up +
                new Vector2(Random.Range(-_turretData.Spread.x, _turretData.Spread.x), Random.Range(-_turretData.Spread.y, _turretData.Spread.y));
            GameObject projectile = _turretData.ProjData.SpawnInstance(_barrel);
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * _turretData.ShotForce, ForceMode2D.Impulse);
            projectile.GetComponent<Projectile>().IgnoreCollisionWith(_mainPart.gameObject);
            _lastShotTime = Time.time;

            Loaded = false;
        }
    }

    public void LoadTurret(AmmoStorage storage)
    {
        if (Loaded)
            return;
        Loaded = storage.GiveAmmo(_turretData.ProjData.ShotCost);
    }

    public bool CanShoot => ReadyToShoot() && Loaded;

    private bool ReadyToShoot()
    {
        if (Time.time >= _lastShotTime + _turretData.FireRate * _fireRateMultiplier)
            return true;
        else
            return false;
    }

    private void OnEnable()
    {
        _barrel = transform.Find("barrel");
    }

    private void LateUpdate()
    {
        if (_mainPart != null)
            transform.position = _mainPart.transform.Find(MainPartData.TurretPlacementStr).position;
    }
}
