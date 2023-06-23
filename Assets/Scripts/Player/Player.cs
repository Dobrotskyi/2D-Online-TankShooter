using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerInputHandler))]
public class Player : MonoBehaviour
{
    private const float _freezeTime = 3f;
    PlayerInput _input;
    PlayerInputHandler _handler;
    private bool _frozen = false;
    public bool Frozen => _frozen;

    private void FreezePlayer()
    {
        _frozen = true;
        StartCoroutine(UnfreezeAfterTime());
    }

    private IEnumerator UnfreezeAfterTime()
    {
        yield return new WaitForSeconds(_freezeTime);
        _frozen = false;
    }

    private void OnEnable()
    {
        _input = GetComponent<PlayerInput>();
        _handler = GetComponent<PlayerInputHandler>();
        GetComponent<Tank>().TankWasDestroyed += FreezePlayer;

        StartCoroutine(TrySetCameraFollow());
    }

    private void OnDisable()
    {
        GetComponent<Tank>().TankWasDestroyed -= FreezePlayer;
    }

    private IEnumerator TrySetCameraFollow()
    {
        Tank tank = GetComponent<Tank>();
        while (tank == null || tank.GetCameraTarget() == null)
        {
            yield return null;
        }
        GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = tank.GetCameraTarget();
    }

    private void FixedUpdate()
    {
        if (_frozen || InGameTimer.GameTime == false)
            return;

        if (_input.Shoot)
            _handler.Shoot();
        if (_input.Move)
            _handler.Move(_input.MoveDirection);
        if (_input.Rotate)
            _handler.Rotate(_input.RotationSide);
    }
}
