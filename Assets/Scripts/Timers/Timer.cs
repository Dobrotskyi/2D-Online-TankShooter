using System;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public abstract class Timer : MonoBehaviour
{
    public event Action TimeIsUp;

    private const int UPDATE_FREQ_SEC = 1;

    protected virtual string Format => @"mm\:ss";
    private TimeSpan _duration;
    [Serializable]
    protected struct TimeSpanFiller
    {
        public int Hours;
        public int Minutes;
        public int Seconds;

        public TimeSpanFiller(int h, int m, int s)
        {
            Hours = h;
            Minutes = m;
            Seconds = s;
        }
    }
    [SerializeField] private TimeSpanFiller _TSFiller = new(0, 1, 30);

    [SerializeField] private TextMeshProUGUI _timerText;
    private PhotonView _view;

    private void Awake()
    {
        _duration = new(_TSFiller.Hours, _TSFiller.Minutes, _TSFiller.Seconds);
        _view = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
            _view.RPC("SendNewTime", RpcTarget.All, _duration.Minutes, _duration.Seconds);
    }

    public virtual void LaunchTimer()
    {
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (_duration > TimeSpan.Zero)
        {
            _duration = _duration.Subtract(new(0, 0, UPDATE_FREQ_SEC));
            _timerText.text = _duration.ToString(Format);
            _view.RPC("SendNewTime", RpcTarget.OthersBuffered, _duration.Minutes, _duration.Seconds);
            yield return new WaitForSeconds(UPDATE_FREQ_SEC);
        }
        _view.RPC("RPC_TimeIsUp", RpcTarget.All);
    }

    [PunRPC]
    protected virtual void RPC_TimeIsUp()
    {
        TimeIsUp?.Invoke();
    }

    [PunRPC]
    protected void SendNewTime(int minutes, int seconds)
    {
        _duration = new(0, minutes, seconds);
        _timerText.text = _duration.ToString(Format);
    }
}
