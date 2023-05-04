using UnityEngine;

[CreateAssetMenu(fileName = "TurretPart", menuName = "Tank/Part/TurretPart", order = 2)]
public class TurretPart : TankPart
{
    [SerializeField] private float _rotationSpeed = 40f;
    [SerializeField] private Vector2 _spread = new Vector2(0.2f, 0.2f);
    [SerializeField] private float _fireRate = 0.25f;
    [SerializeField] private float _shotForce = 15f;
    [SerializeField] private float _minAimingDistance = 1f;
    [SerializeField] private float _durabilityMultiplier = 1f;
    public float DurabilityMultiplier => _durabilityMultiplier;

    private Transform _barrel;
    private float _lastShotTime;
    private Transform _mainPart;
    private float _fireRateMultiplier = 1f;

    public void Shoot(ProjectileSO projectileSO)
    {
        if (IsLoaded())
        {
            Vector2 direction = (Vector2)_barrel.transform.up + new Vector2(Random.Range(-_spread.x, _spread.x), Random.Range(-_spread.y, _spread.y));
            GameObject projectile = projectileSO.SpawnAt(_barrel);
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * _shotForce, ForceMode2D.Impulse);
            projectile.GetComponent<Projectile>().IgnoreCollisionWith(_mainPart.gameObject);
            _fireRateMultiplier = projectileSO.FireRateMultiplier;
            _lastShotTime = Time.time;
        }
    }

    public void AimAtTarget(Vector2 target)
    {
        Vector2 substraction = target - (Vector2)_barrel.transform.position;
        if (substraction.magnitude < _minAimingDistance)
            return;

        Vector2 lookDirection = (target - (Vector2)_barrel.transform.position);
        float angleZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angleZ, _model.transform.forward);
        _model.transform.rotation = Quaternion.Slerp(_model.transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
    }

    public override void SpawnPart(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        base.SpawnPart(parent, activeMonoBehaviour);
        _barrel = _model.transform.Find("Barrel");
        _lastShotTime = Time.time;
        _mainPart = parent.Find("MainPart");
    }

    private bool IsLoaded()
    {
        if (Time.time >= _lastShotTime + _fireRate * _fireRateMultiplier)
            return true;
        else
            return false;
    }
}
