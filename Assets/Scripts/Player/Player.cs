using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerInputHandler))]
public class Player : MonoBehaviour
{
    PlayerInput _input;
    PlayerInputHandler _handler;

    private void OnEnable()
    {
        _input = GetComponent<PlayerInput>();
        _handler = GetComponent<PlayerInputHandler>();
    }

    private void FixedUpdate()
    {
        if (_input.Shoot)
            _handler.Shoot();
        if (_input.Move)
            _handler.Move(_input.MoveDirection);
        if (_input.Rotate)
            _handler.Rotate(_input.RotationSide);
    }
}
