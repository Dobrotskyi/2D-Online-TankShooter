using UnityEngine;

[CreateAssetMenu(fileName = "Tank", menuName = "Tank/Tank")]
public class Tank : ScriptableObject
{
    [SerializeField] private MainPart _mainTankPart;
    [SerializeField] private TurretPart _turretTankPart;
    [SerializeField] private GameObject _projectile;

    public void Move(float direction)
    {
        _mainTankPart.Move(direction);
    }

    public void Rotate(float side)
    {
        _mainTankPart.Rotate(side);
    }

    public void Shoot()
    {
        _turretTankPart.Shoot(_projectile);
    }

    public void SpawnTank(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        _mainTankPart.SpawnPart(parent, activeMonoBehaviour);
        _turretTankPart.SpawnPart(_mainTankPart.InstantiatedModel.transform, activeMonoBehaviour);
    }
}
