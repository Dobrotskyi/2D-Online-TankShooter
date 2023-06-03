using System;
using System.Text;
using UnityEngine;

public class TurretData : PartData
{
    private float _rotationSpeed = 40f;
    private Vector2 _spread = new Vector2(0.2f, 0.2f);
    private float _fireRate = 0.25f;
    private float _shotForce = 15f;
    private float _durabilityMultiplier = 1f;
    private float _damageMult = 1f;
    private ProjectileData _projData;

    public float RotationSpeed => _rotationSpeed;
    public Vector2 Spread => _spread;
    public float FireRate => _fireRate;
    public float ShotForce => _shotForce;
    public float DurabilityMultiplier => _durabilityMultiplier;
    public ProjectileData ProjData => _projData;

    public TurretData(int id, string name, float rotationSpeed, Vector2 spread, float fireRate, float shotForce,
        float dm, float damageMult, Sprite sprite, ProjectileData projData) : base(id, name, sprite)
    {
        _rotationSpeed = rotationSpeed;
        _fireRate = fireRate;
        _shotForce = shotForce;
        _durabilityMultiplier = dm;
        _damageMult = damageMult;
        _spread = spread;
        _projData = projData;
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
        _projData.ApplyDamageMultiplier(_damageMult);
        return turret;
    }

    public override string GetDescription()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Rotation speed: {_rotationSpeed}");
        sb.AppendLine($"Spread: {_spread}");
        sb.AppendLine($"Fire rate: {_fireRate}");
        sb.AppendLine($"Shot force: {_shotForce}");
        sb.AppendLine($"Durability multiplier: {_durabilityMultiplier}");
        sb.AppendLine($"Damage multiplier: {_damageMult}");
        sb.Append(Environment.NewLine);
        sb.AppendLine($"Projectile data:");
        sb.AppendLine(_projData.GetDescription());
        return sb.ToString();
    }
}
