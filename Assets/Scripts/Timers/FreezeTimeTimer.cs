using UnityEngine;
using Photon.Pun;
public class FreezeTimeTimer : Timer
{
    protected override string Format => "%s";
    [SerializeField] private Canvas _freezeTimeCanvas;
    [SerializeField] private GameObject _timerGroup;

    [PunRPC]
    protected override void RPC_TimeIsUp()
    {
        base.RPC_TimeIsUp();
        Destroy(_timerGroup.gameObject);
        _freezeTimeCanvas.gameObject.SetActive(false);
    }

    public override void LaunchTimer()
    {
        _timerGroup.SetActive(true);
        base.LaunchTimer();
    }
}
