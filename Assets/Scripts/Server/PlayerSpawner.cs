using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints = new();
    [SerializeField] private GameObject _playerTank;

    public Vector2 GetRandomSpawnPoint() => _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;

    private void Start()
    {
        int playersAmt = PhotonNetwork.PlayerList.Length;
        PhotonNetwork.Instantiate(_playerTank.name, _spawnPoints[playersAmt - 1].position, Quaternion.identity);
    }
}
