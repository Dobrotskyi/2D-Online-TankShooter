using UnityEngine;

public class Tank : MonoBehaviour, ITakeDamage
{
    [SerializeField] private MainPartData _mainPartData;
    [SerializeField] private TurretPartData _turretPartData;
    [SerializeField] private ProjectileSO _projectile;
    private AmmoStorage _ammoStorage;
    private HealthSystem _health;

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

    public void TakeDamage(int amt) => _health.TakeDamage(amt);

    public void Move(float direction) => _mainPart.Script.Move(direction);

    public void Rotate(float side) => _mainPart.Script.Rotate(side);

    public void Shoot()
    {
        _turretPart.Script.Shoot(_projectile);
        _ammoStorage.LoadTurret(_turretPart.Script);
    }

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
        int maxHealth = Mathf.FloorToInt(_mainPartData.Durability * _turretPartData.DurabilityMultiplier);
        _health = new HealthSystem(maxHealth);
        _health.ZeroHealth += DestroyThisTank;
    }

    private void OnDisable()
    {
        _health.ZeroHealth -= DestroyThisTank;
    }

    private void DestroyThisTank()
    {
        Debug.Log("Health below 0, EXPLOSION");
    }
}
