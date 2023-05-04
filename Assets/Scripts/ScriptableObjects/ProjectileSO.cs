using UnityEngine;

[CreateAssetMenu(fileName = "projectile", menuName = "Projectile", order = 0)]
public class ProjectileSO : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _fireRateMultiplier = 1f;
    [SerializeField] private float _timeOfLife = 5f;
    [SerializeField] private float _damage = 10f;

    public GameObject SpawnAt(Transform origin)
    {
        GameObject projectile = Instantiate(_prefab, origin.position, origin.rotation);
        Destroy(projectile, _timeOfLife);
        return projectile;
    }

    public float FireRateMultiplier => _fireRateMultiplier;
}
