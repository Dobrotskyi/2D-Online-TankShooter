using Photon.Pun;
using UnityEngine;

public class TurretPartBehav : MonoBehaviourPun
{
    private const float MIN_AIMING_DIST = 1f;

    private bool _loaded = false;

    private TurretData _turretData;
    private Transform _barrel;
    private Transform _mainPart;
    private Transform _turretPlacement;
    private float _fireRateMultiplier = 1f;
    private float _lastShotTime = 0f;
    private AmmoStorage _ammoStorage;
    private string _playerNickname;

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
        {
            _mainPart = mainPart;
            _turretPlacement = _mainPart.transform.Find(MainPartData.TurretPlacementStr);
        }
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
        gameObject.transform.rotation = Quaternion.Lerp(_barrel.transform.rotation, rotation, _turretData.RotationSpeed * Time.deltaTime);
    }

    public void Shoot(Vector2 direction, Vector2 pos)
    {
        if (CanShoot)
        {
            GameObject projectileGO = _turretData.ProjData.SpawnInstance(_barrel);
            projectileGO.transform.position = pos;
            projectileGO.GetComponent<Rigidbody2D>().AddForce(direction * _turretData.ShotForce, ForceMode2D.Impulse);
            Projectile proj = projectileGO.GetComponent<Projectile>();
            proj.IgnoreCollisionWith(_mainPart.gameObject);
            proj.ShooterName = _playerNickname;

            _lastShotTime = Time.time;

            _loaded = false;
        }
    }

    public Vector2 GetDirection =>
         (Vector2)_barrel.up +
         new Vector2(Random.Range(-_turretData.Spread.x, _turretData.Spread.x), Random.Range(-_turretData.Spread.y, _turretData.Spread.y));

    public Vector2 GetBarrelPos => _barrel.transform.position;

    public void SetAmmoSource(AmmoStorage ammoStorage)
    {
        _ammoStorage = ammoStorage;
    }

    public bool CanShoot => ReadyToShoot() && _loaded;

    private bool ReadyToShoot()
    {
        if (Time.time >= _lastShotTime + _turretData.FireRate * _fireRateMultiplier)
        {
            _loaded = _ammoStorage.GiveAmmo(_turretData.ProjData.ShotCost);
            return true;
        }
        else
            return false;
    }

    private void OnEnable()
    {
        _barrel = transform.Find("barrel");
        Debug.Log(transform.parent.gameObject.GetComponent<PhotonView>());
        PhotonView parentView = transform.parent.gameObject.GetComponent<PhotonView>();
        _playerNickname = parentView.Owner.NickName;
    }

    private void LateUpdate()
    {
        if (_mainPart != null)
            transform.position = _turretPlacement.position;
    }
}
