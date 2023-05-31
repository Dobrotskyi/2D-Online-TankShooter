using UnityEngine;

public class TurretData : PartData
{
    private float _rotationSpeed = 40f;
    private Vector2 _spread = new Vector2(0.2f, 0.2f);
    private float _fireRate = 0.25f;
    private float _shotForce = 15f;
    private float _durabilityMultiplier = 1f;

    public float RotationSpeed => _rotationSpeed;
    public Vector2 Spread => _spread;
    public float FireRate => _fireRate;
    public float ShotForce => _shotForce;
    public float DurabilityMultiplier => _durabilityMultiplier;

    public TurretData(string name, float rotationSpeed, Vector2 spread, float fireRate, float shotForce, float dm, Sprite sprite) : base(name, sprite)
    {
        _rotationSpeed = rotationSpeed;
        _fireRate = fireRate;
        _shotForce = shotForce;
        _durabilityMultiplier = dm;
    }

    public override GameObject SpawnInstance(Transform parent)
    {
        GameObject turret = base.SpawnInstance(parent);
        SpriteRenderer sr = turret.GetComponent<SpriteRenderer>();
        sr.sortingOrder = 1;

        float halfHeight = sr.bounds.size.y / 2;
        GameObject barrel = new GameObject();
        barrel.name = "barrel";
        barrel.transform.SetParent(turret.transform);
        barrel.transform.position = turret.transform.position;
        barrel.transform.localPosition = new Vector3(0, halfHeight, 0);

        TurretPartBehav behav = turret.AddComponent<TurretPartBehav>();
        return turret;
    }
}
