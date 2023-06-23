using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InGameTimer : MonoBehaviour
{
    public static event Action TimeIsUp;
    public static bool GameTime { get; private set; }

    [SerializeField] private TimeSpan _gameDuration = new(0, 1, 30);
    [SerializeField] private TextMeshProUGUI _timerText;
    private PhotonView _view;
    private const int UPDATE_FREQ_SEC = 1;

    private void Start()
    {
        GameTime = true;
        _view = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(UpdateTimer());

    }

    private IEnumerator UpdateTimer()
    {
        while (_gameDuration > TimeSpan.Zero)
        {
            _gameDuration = _gameDuration.Subtract(new(0, 0, UPDATE_FREQ_SEC));
            _timerText.text = _gameDuration.ToString(@"mm\:ss");
            _view.RPC("SendNewTime", RpcTarget.OthersBuffered, _gameDuration.Minutes, _gameDuration.Seconds);
            yield return new WaitForSeconds(UPDATE_FREQ_SEC);
        }
        _view.RPC("RPC_TimeIsUp", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_TimeIsUp()
    {
        GameTime = false;
        TimeIsUp?.Invoke();
    }

    [PunRPC]
    private void SendNewTime(int minutes, int seconds)
    {
        _gameDuration = new(0, minutes, seconds);
        _timerText.text = _gameDuration.ToString(@"mm\:ss");
    }

}
