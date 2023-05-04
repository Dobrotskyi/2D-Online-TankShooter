using UnityEngine;

[CreateAssetMenu(fileName = "Tank", menuName = "Tank/Tank")]
public class Tank : ScriptableObject
{
    [SerializeField] private MainPart _mainTankPart;
    [SerializeField] private TurretPart _turretTankPart;
    [SerializeField] private ProjectileSO _projectile;
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
            if(_health > _maxHealth)
                _health = _maxHealth;
            else if (_health <= 0)
                DestroyThisTank();
        }
    }

    public void Move(float direction)
    {
        _mainTankPart.Move(direction);
    }

    public void Rotate(float side)
    {
        _mainTankPart.Rotate(side);
    }

    public void Shoot()
    {
        _turretTankPart.Shoot(_projectile);
    }

    public void Aim(Vector2 target)
    {
        _turretTankPart.AimAtTarget(target);
    }

    public void SpawnTank(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        _mainTankPart.SpawnPart(parent, activeMonoBehaviour);
        _turretTankPart.SpawnPart(parent, activeMonoBehaviour);
        _maxHealth = _mainTankPart.Durability * _turretTankPart.DurabilityMultiplier;
        Health = _maxHealth;
    }

    public Transform GetCameraTarget()
    {
        return _mainTankPart.InstantiatedModel.transform;
    }

    public void LateUpdate()
    {
        _turretTankPart.InstantiatedModel.transform.position = _mainTankPart.TurretPlacement.position;
    }

    private void DestroyThisTank()
    {
        Debug.Log("Health below 0");
    }
}
