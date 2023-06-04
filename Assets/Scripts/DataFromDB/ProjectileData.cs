using System.Text;
using UnityEngine;

public class ProjectileData : PartData
{
    private int _damage;
    private float _timeOfLife;
    private int _ammoCost;

    public float Damage => _damage;
    public float TimeOfLife => _timeOfLife;
    public int ShotCost => _ammoCost;

    public ProjectileData(int id, string name, int damage, float timeOfLife, int cost, Sprite sprite) : base(id, name, sprite)
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
        projectile.AddComponent<Projectile>().SetInfo(_damage, _timeOfLife);
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
