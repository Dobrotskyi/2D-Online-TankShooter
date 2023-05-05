using UnityEngine;

[CreateAssetMenu(fileName = "projectile", menuName = "Projectile", order = 0)]
public class ProjectileSO : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _fireRateMultiplier = 1f;
    [SerializeField] private float _timeOfLife = 5f;
    [SerializeField] private int _damage = 10;
    public float FireRateMultiplier => _fireRateMultiplier;

    public GameObject SpawnAt(Transform origin)
    {
        GameObject projectile = Instantiate(_prefab, origin.position, origin.rotation);
        projectile.AddComponent(typeof(Projectile));
        projectile.GetComponent<Projectile>().SetDamage(_damage);
        Destroy(projectile, _timeOfLife);
        return projectile;
    }
}
