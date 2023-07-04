using Photon.Pun;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerInputHandler))]
public class Player : MonoBehaviour
{
    private const float _freezeTime = 3f;
    private PlayerInput _input;
    private PlayerInputHandler _handler;
    private FreezeTimeTimer _freezeTimeTimer;
    private PhotonView _view;

    private bool _frozen = true;
    public bool Frozen => _frozen;

    private void FreezePlayer()
    {
        _view.RPC("ChangeFreezeState", RpcTarget.All, true);
        StartCoroutine(UnfreezeAfterTime(_freezeTime));
    }

    private IEnumerator UnfreezeAfterTime(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        InstantUnfreeze();
    }

    private void OnEnable()
    {
        _view = GetComponent<PhotonView>();
        _input = GetComponent<PlayerInput>();
        _handler = GetComponent<PlayerInputHandler>();
        GetComponent<Tank>().TankWasDestroyed += FreezePlayer;
        _freezeTimeTimer = FindObjectOfType<FreezeTimeTimer>();
        _freezeTimeTimer.TimeIsUp += InstantUnfreeze;

        StartCoroutine(TrySetCameraFollow());
    }

    private void InstantUnfreeze() => _view.RPC("ChangeFreezeState", RpcTarget.All, false);

    [PunRPC]
    private void ChangeFreezeState(bool frozen) => _frozen = frozen;

    private void OnDisable()
    {
        GetComponent<Tank>().TankWasDestroyed -= FreezePlayer;
        _freezeTimeTimer.TimeIsUp -= InstantUnfreeze;
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
        if (_frozen || GameTimeTimer.GameTime == false)
            return;

        if (_input.Shoot)
            _handler.Shoot();
        if (_input.Move)
            _handler.Move(_input.MoveDirection);
        if (_input.Rotate)
            _handler.Rotate(_input.RotationSide);
    }
}
