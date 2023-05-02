using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private MainPart _mainTankPart;
    private void OnEnable()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        _mainTankPart = GetComponent<Player>().GetMainPart();
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
        _mainTankPart.Move(direction);
    }

    private void Rotate(float side)
    {
        _mainTankPart.Rotate(side);
    }

    private void Shoot()
    {
        Debug.Log("Boom");
    }
}
