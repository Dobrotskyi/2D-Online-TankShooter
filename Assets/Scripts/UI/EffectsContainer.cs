using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsContainer : MonoBehaviour
{
    [SerializeField] private GameObject _projectileHit;
    public GameObject ProjectileHit => _projectileHit;
}
