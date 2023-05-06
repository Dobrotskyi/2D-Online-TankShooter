using UnityEngine;

[CreateAssetMenu(fileName = "MainPart", menuName = "Tank/Part/MainPart", order = 0)]
public class MainPart : TankPartData
{
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _angularSpeed;
    [SerializeField] private float _durability = 300f;

    public float Acceleration => _acceleration;
    public float MaxSpeed => _maxSpeed;
    public float AngularSpeed => _angularSpeed;
    public float Durability => _durability;

    public override GameObject SpawnPart(Transform parent)
    {
        GameObject mainPart = base.SpawnPart(parent);
        mainPart.AddComponent(typeof(MainPartBehav));
        mainPart.AddComponent(typeof(TankCollisionHandler));
        return mainPart;
    }
}
