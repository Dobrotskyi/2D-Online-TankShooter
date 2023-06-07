using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints = new();
    [SerializeField] private GameObject _playerTank;

    private void Start()
    {
        int playersAmt = PhotonNetwork.PlayerList.Length;
        PhotonNetwork.Instantiate(_playerTank.name, _spawnPoints[playersAmt - 1].position, Quaternion.identity);
        _spawnPoints.RemoveAt(0);
    }
}
