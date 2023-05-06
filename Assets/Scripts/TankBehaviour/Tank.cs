using UnityEngine;

public class Tank : MonoBehaviour, ITakeDamage
{
    [SerializeField] private MainPart _mainTankPartData;
    [SerializeField] private TurretPart _turretTankPartData;
    [SerializeField] private ProjectileSO _projectile;
    private GameObject _spawnedMainPart;
    private MainPartBehav _mainPart;
    private GameObject _spawnedTurretPart;
    private TurretPartBehav _turretPart;
    private float _maxHealth;
    private float _health = 0;

    public float Health
    {
        get
        {
            return _health;
        }
        private set
        {
            _health = value;
            if (_health > _maxHealth)
                _health = _maxHealth;
            else if (_health <= 0)
                DestroyThisTank();
        }
    }

    public void TakeDamage(int amt) => Health -= amt;

    public void Move(float direction) => _mainPart.Move(direction);

    public void Rotate(float side) => _mainPart.Rotate(side);

    public void Shoot() => _turretPart.Shoot(_projectile);

    public void Aim(Vector2 target) => _turretPart.AimAtTarget(target);

    public Transform GetCameraTarget() => _spawnedMainPart.transform;

    public void RestoreAmmo(int amt)
    {
        Debug.Log($"ammo was added - + {amt}");
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
        _spawnedMainPart = _mainTankPartData.SpawnPart(transform);
        _mainPart = _spawnedMainPart.GetComponent<MainPartBehav>();
        _mainPart.SetData(_mainTankPartData);

        _spawnedTurretPart = _turretTankPartData.SpawnPart(transform);
        _turretPart = _spawnedTurretPart.GetComponent<TurretPartBehav>();
        _turretPart.SetData(_turretTankPartData);

        _maxHealth = _mainTankPartData.Durability * _turretTankPartData.DurabilityMultiplier;
        Health = _maxHealth;
    }

    private void DestroyThisTank()
    {
        Debug.Log("Health below 0");
    }
}
