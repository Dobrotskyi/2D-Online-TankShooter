using UnityEngine;

[CreateAssetMenu(fileName = "TurretPart", menuName = "Tank/Part/TurretPart", order = 2)]
public class TurretPart : TankPartData
{
    [SerializeField] private float _rotationSpeed = 40f;
    [SerializeField] private Vector2 _spread = new Vector2(0.2f, 0.2f);
    [SerializeField] private float _fireRate = 0.25f;
    [SerializeField] private float _shotForce = 15f;
    [SerializeField] private float _minAimingDistance = 1f;
    [SerializeField] private float _durabilityMultiplier = 1f;
    [SerializeField] private float _fireRateMultiplier = 1f;

    public float RotationSpeed => _rotationSpeed;
    public Vector2 Spread => _spread;
    public float FireRate => _fireRate;
    public float ShotForce => _shotForce;
    public float MinAimingDistance => _minAimingDistance;
    public float DurabilityMultiplier => _durabilityMultiplier;
    public float FireRateMultiplier => _fireRateMultiplier;

    public override GameObject SpawnPart(Transform parent)
    {
        GameObject part = base.SpawnPart(parent);
        part.AddComponent(typeof(TurretPartBehav));
        return part;
    }
}
