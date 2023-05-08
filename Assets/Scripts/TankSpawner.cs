using UnityEngine;

public class TankSpawner : MonoBehaviour
{
    [SerializeField] Tank _tank;

    public Tank Tank => _tank;

    private void OnEnable()
    {
        //_tank.SpawnTank();
    }
}
