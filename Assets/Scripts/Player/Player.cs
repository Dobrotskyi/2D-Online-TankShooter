using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Tank _tank;

    public Tank Tank => _tank;

    private void OnEnable()
    {
        _tank.SpawnTank(transform, this);
    }
}
