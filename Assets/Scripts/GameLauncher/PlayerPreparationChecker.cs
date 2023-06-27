using System;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PlayerPreparationChecker : MonoBehaviourSingleton<PlayerPreparationChecker>
{
    public event Action GameIsReadyToLaunch;

    private int ConnectedPlayers => PhotonNetwork.PlayerList.Length;
    private int _loadedTanksCounter;
    private PhotonView _view;

    private int _playersReady;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        Tank.TankLoaded += LoadedTankPlus;
    }

    private void OnDisable()
    {
        Tank.TankLoaded -= LoadedTankPlus;
    }

    private void LoadedTankPlus()
    {
        Debug.Log($"LoadedTankPlus++ {ConnectedPlayers}");
        _loadedTanksCounter++;
        if (_loadedTanksCounter == ConnectedPlayers)       
            _view.RPC("InvokePlayerReady", RpcTarget.MasterClient);
        
    }

    [PunRPC]
    private void InvokePlayerReady()
    {
        Debug.Log("AddReadyPlayer");
        _playersReady++;
        if (_playersReady == ConnectedPlayers)
            _view.RPC("SendGameIsReady", RpcTarget.All);
    }

    [PunRPC]
    private void SendGameIsReady()
    {
        Debug.Log("GameIsReadyToLaunch");
        GameIsReadyToLaunch?.Invoke();
    }
}
