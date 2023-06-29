using Photon.Pun;
using System;

public class GameTimeTimer : Timer
{
    public static bool GameTime { get; private set; } = true;

    private FreezeTimeTimer _timer;

    public override void LaunchTimer()
    {
        GameTime = true;
        base.LaunchTimer();
    }

    private void Start()
    {
        _timer = FindObjectOfType<FreezeTimeTimer>();
        _timer.TimeIsUp += LaunchTimer;
    }

    private void OnDisable()
    {
        _timer.TimeIsUp += LaunchTimer;
    }

    [PunRPC]
    protected override void RPC_TimeIsUp()
    {
        GameTime = false;
        base.RPC_TimeIsUp();
    }
}
