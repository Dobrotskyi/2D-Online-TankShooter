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
        GameObject obj = new GameObject();
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = _sprite;

        TurretPartBehav behav = obj.AddComponent<TurretPartBehav>();
        behav.SetData(this);

        obj.AddComponent<BoxCollider2D>();

        obj.transform.SetParent(parent);

        return Object.Instantiate(obj, parent.position, parent.rotation); ;
    }
}
