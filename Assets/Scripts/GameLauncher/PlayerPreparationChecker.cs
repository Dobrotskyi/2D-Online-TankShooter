using System;
using UnityEngine;
using Photon.Pun;

public class PlayerPreparationChecker : MonoBehaviourSingleton<PlayerPreparationChecker>
{
    public event Action GameIsReadyToLaunch;

    private int ConnectedPlayers => PhotonNetwork.PlayerList.Length;
    private int _loadedTanksCounter;
    private bool _tanksAreLoaded = false;
    private int _spawnedTanksCounter;
    private bool _tanksAreSpawned = false;
    private PhotonView _view;

    private int _playersReady;

    private void OnEnable()
    {
        Tank.TankLoaded += LoadedTankPlus;
        PlayerSpawnpoint.PlayerSpawned += SpawnedTankPlus;
    }

    private void OnDisable()
    {
        Tank.TankLoaded -= LoadedTankPlus;
        PlayerSpawnpoint.PlayerSpawned -= SpawnedTankPlus;
    }

    private void LoadedTankPlus()
    {
        _loadedTanksCounter++;
        if (_loadedTanksCounter == ConnectedPlayers)
        {
            _tanksAreLoaded = true;
            CheckIfReady();
        }
    }

    private void SpawnedTankPlus()
    {
        _spawnedTanksCounter++;
        if (_spawnedTanksCounter == ConnectedPlayers)
        {
            _tanksAreSpawned = true;
            CheckIfReady();
        }

    }

    private void CheckIfReady()
    {
        if (_tanksAreLoaded && _tanksAreSpawned)
            _view.RPC("InvokePlayerReady", RpcTarget.MasterClient);

    }

    [PunRPC]
    private void InvokePlayerReady() => AddReadyPlayer();

    private void AddReadyPlayer()
    {
        _playersReady++;
        if(_playersReady == ConnectedPlayers)
            GameIsReadyToLaunch?.Invoke();
    }
}
