using UnityEngine;

public class Tank : MonoBehaviour, ITakeDamage
{
    [SerializeField] private MainPartData _mainPartData;
    [SerializeField] private TurretPartData _turretPartData;
    [SerializeField] private ProjectileSO _projectile;
    private AmmoStorage _ammoStorage;

    private struct MainPart
    {
        public GameObject SpawnedObj { get; private set; }
        public MainPartBehav Script { get; private set; }

        public MainPart(GameObject spawnedPart)
        {
            SpawnedObj = spawnedPart;
            Script = spawnedPart.GetComponent<MainPartBehav>();
        }
    }

    private struct TurretPart
    {
        public GameObject SpawnedObj { get; private set; }
        public TurretPartBehav Script { get; private set; }

        public TurretPart(GameObject spawnedPart)
        {
            SpawnedObj = spawnedPart;
            Script = spawnedPart.GetComponent<TurretPartBehav>();
        }
    }

    private MainPart _mainPart;
    private TurretPart _turretPart;

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

    public void Move(float direction) => _mainPart.Script.Move(direction);

    public void Rotate(float side) => _mainPart.Script.Rotate(side);

    public void Shoot() => _turretPart.Script.Shoot(_projectile);

    public void Aim(Vector2 target) => _turretPart.Script.AimAtTarget(target);

    public Transform GetCameraTarget() => _mainPart.SpawnedObj.transform;

    public void RestoreAmmo(int amt)
    {
        Debug.Log($"ammo was added - + {amt}");
        _ammoStorage.RessuplyAmmo(amt);
    }

    public void LateUpdate()
    {
        _turretPart.SpawnedObj.transform.position = _mainPart.SpawnedObj.transform.Find("TurretPlacement").position;
    }

    private void OnEnable()
    {
        SpawnTank();
    }

    private void SpawnTank()
    {
        _mainPart = new MainPart(_mainPartData.SpawnPart(transform));
        _turretPart = new TurretPart(_turretPartData.SpawnPart(transform));

        _mainPart.Script.SetData(_mainPartData);
        _turretPart.Script.SetData(_turretPartData);
        _turretPart.Script.AttachToBase(_mainPart.SpawnedObj.transform);
        _ammoStorage = new AmmoStorage(_mainPartData.AmmoStorage);
        _turretPart.Script.SetAmmoStorage(_ammoStorage);

        _maxHealth = _mainPartData.Durability * _turretPartData.DurabilityMultiplier;
        Health = _maxHealth;
    }

    private void DestroyThisTank()
    {
        Debug.Log("Health below 0");
    }
}
