using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] private MainPartData _mainTankPart;
    [SerializeField] private TurretPartData _turretTankPart;
    [SerializeField] private ProjectileSO _projectile;
    private GameObject _spawnedMainPart;
    private MainTankPart _mainPart;
    private GameObject _spawnedTurretPart;
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
            _health += value;
            if (_health > _maxHealth)
                _health = _maxHealth;
            else if (_health <= 0)
                DestroyThisTank();
        }
    }

    public void Move(float direction)
    {
        _mainPart.Move(direction);
    }

    public void Rotate(float side)
    {
        _mainPart.Rotate(side);
    }

    public void Shoot()
    {
        //_turretTankPart.Shoot(_projectile);
    }

    public void Aim(Vector2 target)
    {
        //_turretTankPart.AimAtTarget(target);
    }

    public Transform GetCameraTarget()
    {
        return _spawnedMainPart.transform;
    }

    public void LateUpdate()
    {
        _spawnedTurretPart.transform.position = _spawnedMainPart.transform.Find("TurretPlacement").position;
        //_turretTankPart.Shoot(_projectile);
        _mainPart.Move(transform.forward.magnitude);
    }

    private void OnEnable()
    {
        SpawnTank();
    }

    private void SpawnTank()
    {
        _spawnedMainPart = _mainTankPart.SpawnPart(transform);
        _mainPart = _spawnedMainPart.GetComponent<MainTankPart>();
        _mainPart.SetData(_mainTankPart);
        _spawnedTurretPart = _turretTankPart.SpawnPart(transform);
        _maxHealth = _mainTankPart.Durability * _turretTankPart.DurabilityMultiplier;
        Health = _maxHealth;
    }


    private void DestroyThisTank()
    {
        Debug.Log("Health below 0");
    }
}
