using UnityEngine;

public class Tank : MonoBehaviour, ITakeDamage
{
    [SerializeField] private MainPartData _mainPartData;
    [SerializeField] private TurretPartData _turretPartData;
    [SerializeField] private ProjectileSO _projectile;
    [SerializeField] private PropertyBar[] _bars;
    private AmmoStorage _ammoStorage;
    private Health _health;

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

    private void OnEnable()
    {
        SpawnTank();
        SetPropertyBars();
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
        _health = new Health(maxHealth);
        _health.ZeroHealth += DestroyThisTank;
    }

    private void SetPropertyBars()
    {
        if (_bars.Length != 0)
        {
            //Temporary
            foreach (var bar in _bars)
                bar.transform.parent = _mainPart.SpawnedObj.transform;

            _bars[0].SetProperty(_health);
        }
    }

    private void OnDisable()
    {
        _health.ZeroHealth -= DestroyThisTank;
    }

    private void DestroyThisTank()
    {
        Debug.Log("Destroying this tank");
    }
}
