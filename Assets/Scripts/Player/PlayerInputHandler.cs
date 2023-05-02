using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private Tank _tank;

    private void OnEnable()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        _tank = GetComponent<Player>().Tank;
        input.Move += Move;
        input.Rotate += Rotate;
        input.Shoot += Shoot;
    }

    private void OnDisable()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        input.Move -= Move;
        input.Rotate -= Rotate;
        input.Shoot -= Shoot;
    }

    private void Move(float direction)
    {
       _tank.Move(direction);
    }

    private void Rotate(float side)
    {
        _tank.Rotate(side);
    }

    private void Shoot()
    {
        _tank.Shoot();
    }
}