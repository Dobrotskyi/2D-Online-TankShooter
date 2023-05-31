using UnityEngine;

public class MainPartData : PartData
{
    public static string TurretPlacementStr = "turretPlacement";

    private const float MASS = 50f;
    private const int DRAG = 5;

    private float _acceleration = -1;
    private float _maxSpeed = -1;
    private float _angularSpeed = -1;
    private float _durability = -1;
    private int _ammoStorage = -1;
    private Vector2 _turretPlacement;

    public float Acceleration => _acceleration;
    public float MaxSpeed => _maxSpeed;
    public float AngularSpeed => _angularSpeed;
    public float Durability => _durability;
    public int AmmoStorage => _ammoStorage;
    public Vector2 TurretPlacement => _turretPlacement;

    public MainPartData(string name, float acceleration, float maxSpeed, float angularSpeed, float durability, int ammo, Vector2 turretPos, Sprite sprite) : base(name, sprite)
    {
        _acceleration = acceleration;
        _maxSpeed = maxSpeed;
        _angularSpeed = angularSpeed;
        _durability = durability;
        _ammoStorage = ammo;
        _turretPlacement = turretPos;
    }

    public override GameObject SpawnInstance(Transform parent)
    {
        GameObject mainPart = base.SpawnInstance(parent);
        Rigidbody2D rb = mainPart.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.mass = MASS;
        rb.drag = DRAG;
        rb.angularDrag = DRAG;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        mainPart.AddComponent<BoxCollider2D>();

        GameObject turretPlacement = new();
        turretPlacement.transform.SetParent(mainPart.transform);
        turretPlacement.transform.localPosition = _turretPlacement;
        turretPlacement.name = TurretPlacementStr;

        mainPart.AddComponent<MainPartBehav>().SetData(this);
        mainPart.AddComponent<TankCollisionHandler>();
        return mainPart;
    }

}
