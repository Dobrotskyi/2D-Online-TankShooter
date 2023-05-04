using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] Tank _tank;

    public Tank Tank => _tank;

    private void OnEnable()
    {
        _tank.SpawnTank(transform, this);
    }
}
