using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.UIElements.ToolbarMenu;

public class TurretData
{
    private const string phpUrl = "http://localhost/topdowntankshooter/get_selected_turret.php";

    private float _rotationSpeed = 40f;
    private Vector2 _spread = new Vector2(0.2f, 0.2f);
    private float _fireRate = 0.25f;
    private float _shotForce = 15f;
    private float _durabilityMultiplier = 1f;
    private string _name = null;
    private Sprite _sprite;

    public float RotationSpeed => _rotationSpeed;
    public Vector2 Spread => _spread;
    public float FireRate => _fireRate;
    public float ShotForce => _shotForce;
    public float DurabilityMultiplier => _durabilityMultiplier;
    public string Name => _name;
    public Sprite Sprite => _sprite;

    public TurretData(string name, float rotationSpeed, Vector2 spread, float fireRate, float shotForce, float dm, Sprite texture)
    {
        _rotationSpeed = rotationSpeed;
        _fireRate = fireRate;
        _shotForce = shotForce;
        _durabilityMultiplier = dm;
        _name = name;
        _sprite = texture;
    }

    public GameObject SpawnInstance(Transform parent)
    {
        GameObject turret = new();
        turret.transform.SetParent(parent);
        turret.transform.localPosition = Vector3.zero;
        turret.transform.localRotation = parent.transform.localRotation;
        SpriteRenderer sr = turret.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 1;
        sr.sprite = _sprite;
        sr.transform.localScale = new Vector2(1.3f, 1.3f);

        float halfHeight = sr.bounds.size.y / 2;
        GameObject barrel = new GameObject();
        barrel.name = "barrel";
        barrel.transform.SetParent(turret.transform);
        barrel.transform.position = turret.transform.position;
        barrel.transform.localPosition = new Vector3(0, halfHeight, 0);

        TurretPartBehav behav = turret.AddComponent<TurretPartBehav>();
        turret.transform.SetParent(parent);

        return turret;
    }
}
