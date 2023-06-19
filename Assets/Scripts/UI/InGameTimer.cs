using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InGameTimer : MonoBehaviour
{
    public event Action TimeIsUp;

    [SerializeField] private TimeSpan _gameDuration = new(0, 1, 30);
    [SerializeField] private TextMeshProUGUI _timerText;
    private PhotonView _view;
    private const float UPDATE_FREQ = 0.5f;


    private void Start()
    {
        _view = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(UpdateTimer());
            StartCoroutine(SendMastersTime());
        }

    }

    private IEnumerator UpdateTimer()
    {
        while (_gameDuration > TimeSpan.Zero)
        {
            _gameDuration = _gameDuration.Subtract(new(0, 0, 1));
            _timerText.text = _gameDuration.ToString(@"mm\:ss");
            yield return new WaitForSeconds(1);
        }
        TimeIsUp?.Invoke();
    }

    private IEnumerator SendMastersTime()
    {
        while (_gameDuration > TimeSpan.Zero)
        {
            _view.RPC("SendNewTime", RpcTarget.OthersBuffered, _gameDuration.Minutes, _gameDuration.Seconds);
            yield return new WaitForSeconds(1);
        }
    }

    [PunRPC]
    private void SendNewTime(int minutes, int seconds)
    {
        _gameDuration = new(0, minutes, seconds);
        _timerText.text = _gameDuration.ToString(@"mm\:ss");
    }

}
