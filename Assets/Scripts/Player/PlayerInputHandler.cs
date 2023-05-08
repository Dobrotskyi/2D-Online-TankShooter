using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private Tank _tank;
    private Camera _cam;

    private void OnEnable()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        _tank = GetComponent<Tank>();
        GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = _tank.GetCameraTarget();

        _cam = Camera.main;
    }

    private void Update()
    {
        _tank.Aim(_cam.ScreenToWorldPoint(Input.mousePosition));
    }

    public void Move(float direction) => _tank.Move(direction);

    public void Rotate(float side) => _tank.Rotate(side);

    public void Shoot() => _tank.Shoot();
}
