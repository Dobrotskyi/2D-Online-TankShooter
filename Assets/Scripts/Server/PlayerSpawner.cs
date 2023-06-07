using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private List<Vector2> _spawnPoints = new();
    [SerializeField] private GameObject _playerTank;

    private void Start()
    {
        PhotonNetwork.Instantiate(_playerTank.name, _spawnPoints[0], Quaternion.identity);
        _spawnPoints.RemoveAt(0);
    }
}
