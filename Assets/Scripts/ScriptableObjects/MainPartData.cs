using UnityEngine;

[CreateAssetMenu(fileName = "MainPart", menuName = "Tank/Part/MainPart", order = 0)]
public class MainPartData : TankPartData
{
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _angularSpeed;
    [SerializeField] private float _durability = 300f;
    [SerializeField] private int _ammoStorage = 30;

    public float Acceleration => _acceleration;
    public float MaxSpeed => _maxSpeed;
    public float AngularSpeed => _angularSpeed;
    public float Durability => _durability;
    public int AmmoStorage => _ammoStorage;

    public override GameObject SpawnPart(Transform parent)
    {
        GameObject mainPart = base.SpawnPart(parent);
        mainPart.AddComponent(typeof(MainPartBehav));
        mainPart.AddComponent(typeof(TankCollisionHandler));
        return mainPart;
    }
}
