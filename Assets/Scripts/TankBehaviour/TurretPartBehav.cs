using UnityEngine;

public class TurretPartBehav : MonoBehaviour
{
    private const float MIN_AIMING_DIST = 1f;

    private TurretPartData _data;
    private Transform _barrel;
    private Transform _mainPart;
    private float _fireRateMultiplier = 1f;
    private float _lastShotTime = 0f;

    public bool Loaded = false;

    public void SetData(TurretPartData data)
    {
        if (_data == null)
            _data = data;
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
        gameObject.transform.rotation = Quaternion.Slerp(_barrel.transform.rotation, rotation, _data.RotationSpeed * Time.deltaTime);
    }

    public void Shoot(ProjectileSO projectileSO)
    {
        if (CanShoot)
        {
            Vector2 direction = (Vector2)_barrel.up +
                new Vector2(Random.Range(-_data.Spread.x, _data.Spread.x), Random.Range(-_data.Spread.y, _data.Spread.y));
            GameObject projectile = projectileSO.SpawnAt(_barrel);
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * _data.ShotForce, ForceMode2D.Impulse);
            projectile.GetComponent<Projectile>().IgnoreCollisionWith(_mainPart.gameObject);
            _fireRateMultiplier = projectileSO.FireRateMultiplier;
            _lastShotTime = Time.time;

            Loaded = false;
        }
    }

    public bool CanShoot => ReadyToShoot() && Loaded;

    private bool ReadyToShoot()
    {
        if (Time.time >= _lastShotTime + _data.FireRate * _fireRateMultiplier)
            return true;
        else
            return false;
    }

    private void OnEnable()
    {
        _barrel = transform.Find("Barrel");
    }

    private void LateUpdate()
    {
        transform.position = _mainPart.transform.Find("TurretPlacement").position;
    }
}
