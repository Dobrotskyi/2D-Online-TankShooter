using UnityEngine;

public class EffectsContainer : MonoBehaviour
{
    [SerializeField] private GameObject _projectileHit;
    [SerializeField] private ParticleSystem _engineSmoke;
    public GameObject ProjectileHit => _projectileHit;
    public ParticleSystem EngineSmoke => _engineSmoke;
}
