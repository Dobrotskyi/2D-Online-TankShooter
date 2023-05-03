using UnityEngine;

[CreateAssetMenu(fileName = "TurretPart", menuName = "Tank/Part/TurretPart", order = 2)]
public class TurretPart : TankPart
{
    [SerializeField] float _angularSpeed = 40f;
    [SerializeField] Vector2 _spread = new Vector2(0.2f, 0.2f);
    [SerializeField] float _fireRate = 0.25f;
    [SerializeField] float _shotForce = 15f;
    private Transform _barrel;
    private float _lastShotTime;
    private Transform _mainPart;
    private Rigidbody2D _rb;

    public void Shoot(GameObject projectilePrefab)
    {
        if (Time.time >= _lastShotTime + _fireRate)
        {
            Vector2 direction = (Vector2)_barrel.transform.up + new Vector2(Random.Range(-_spread.x, _spread.x), Random.Range(-_spread.y, _spread.y));
            GameObject projectile = Instantiate(projectilePrefab, _barrel.position, _barrel.rotation);
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * _shotForce, ForceMode2D.Impulse);
            projectile.GetComponent<Projectile>().IgnoreCollisionWith(_mainPart.gameObject);
            _lastShotTime = Time.time;
        }
    }

    public void AimAtTarget(Vector2 target)
    {
        Vector2 lookDirection = target - (Vector2)_model.transform.position;
        float angleZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angleZ);
        _model.transform.rotation = Quaternion.Lerp(_model.transform.rotation, rotation, _angularSpeed * Time.deltaTime);
    }

    public override void SpawnPart(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        base.SpawnPart(parent, activeMonoBehaviour);
        _barrel = _model.transform.Find("Barrel");
        _lastShotTime = Time.time;
        _mainPart = parent;
        _rb = _mainPart.GetComponent<Rigidbody2D>();
    }
}
