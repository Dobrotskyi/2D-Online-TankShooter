using System.Text;
using UnityEngine;

public class ProjectileData : PartData
{
    private int _damage;
    private float _timeOfLife;
    private int _ammoCost;

    public float Damage => _damage;
    public float TimeOfLife => _timeOfLife;
    public int AmmoCost => _ammoCost;

    public ProjectileData(string name, int damage, float timeOfLife, int cost, Sprite sprite) : base(name, sprite)
    {
        _damage = damage;
        _timeOfLife = timeOfLife;
        _ammoCost = cost;
    }

    public void ApplyDamageMultiplier(float dmgMultiplier) => _damage = (int)(_damage * dmgMultiplier);

    public override GameObject SpawnInstance(Transform parent)
    {
        GameObject projectile = base.SpawnInstance(parent);
        projectile.transform.SetParent(null);

        Rigidbody2D rb = projectile.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        projectile.AddComponent<BoxCollider2D>();
        projectile.AddComponent<Projectile>().SetDamage(_damage);
        projectile.name = _name;
        projectile.tag = "Projectile";
        return projectile;
    }

    public override string GetDescription()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Damage: {_damage}");
        sb.AppendLine($"Time of life: {_timeOfLife}");
        sb.AppendLine($"Ammo cost: {_ammoCost}");

        return sb.ToString();
    }

}