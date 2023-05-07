using UnityEngine;

public class Tank : MonoBehaviour, ITakeDamage
{
    [SerializeField] private MainPartData _mainPartData;
    [SerializeField] private TurretPartData _turretPartData;
    [SerializeField] private ProjectileSO _projectile;
    private GameObject _spawnedMainPart;
    private MainPartBehav _mainPartBehav;
    private GameObject _spawnedTurretPart;
    private TurretPartBehav _turretPartBehav;
    private AmmoStorage _ammoStorage;

    private float _maxHealth;
    private float _health = 0;

    public float Health
    {
        get => _health;

        private set
        {
            _health = value;
            if (_health > _maxHealth)
                _health = _maxHealth;
            else if (_health <= 0)
                DestroyThisTank();
            Debug.Log(_health);
        }
    }

    public void TakeDamage(int amt) => Health -= amt;

    public void Move(float direction) => _mainPartBehav.Move(direction);

    public void Rotate(float side) => _mainPartBehav.Rotate(side);

    public void Shoot() => _turretPartBehav.Shoot(_projectile);

    public void Aim(Vector2 target) => _turretPartBehav.AimAtTarget(target);

    public Transform GetCameraTarget() => _spawnedMainPart.transform;

    public void RestoreAmmo(int amt)
    {
        Debug.Log($"ammo was added - + {amt}");
        _ammoStorage.RessuplyAmmo(amt);
    }

    public void LateUpdate()
    {
        _spawnedTurretPart.transform.position = _spawnedMainPart.transform.Find("TurretPlacement").position;
    }

    private void OnEnable()
    {
        SpawnTank();
    }

    private void SpawnTank()
    {
        _spawnedMainPart = _mainPartData.SpawnPart(transform);
        _mainPartBehav = _spawnedMainPart.GetComponent<MainPartBehav>();
        _mainPartBehav.SetData(_mainPartData);

        _spawnedTurretPart = _turretPartData.SpawnPart(transform);
        _turretPartBehav = _spawnedTurretPart.GetComponent<TurretPartBehav>();
        _turretPartBehav.SetData(_turretPartData);
        _turretPartBehav.AttachToBase(_mainPartBehav.transform);

        _ammoStorage = new AmmoStorage(_mainPartData.AmmoStorage);
        _turretPartBehav.SetAmmoStorage(_ammoStorage);

        _maxHealth = _mainPartData.Durability * _turretPartData.DurabilityMultiplier;
        Health = _maxHealth;
    }

    private void DestroyThisTank()
    {
        Debug.Log("Health below 0");
    }
}
