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

    public void Aim(Vector2 target)
    {
        _turretTankPart.AimAtTarget(target);
    }

    public void SpawnTank(Transform parent, MonoBehaviour activeMonoBehaviour)
    {
        _mainTankPart.SpawnPart(parent, activeMonoBehaviour);
        _turretTankPart.SpawnPart(parent, activeMonoBehaviour);
        GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = _mainTankPart.InstantiatedModel.transform;
    }

    public void LateUpdate()
    {
        _turretTankPart.InstantiatedModel.transform.position = _mainTankPart.TurretPlacement.position;
    }
}
