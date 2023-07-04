using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private List<PlayerSpawnpoint> _spawnPoints = new();
    [SerializeField] private GameObject _playerTank;
    private PhotonView _view;

    public Vector2 GetRandomSpawnPoint() => _spawnPoints[0].transform.position;//_spawnPoints[Random.Range(0, _spawnPoints.Count)].transform.position;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        int playersAmt = PhotonNetwork.PlayerList.Length;
        if (PhotonNetwork.IsMasterClient)
            _view.RPC("RPC_Spawn", RpcTarget.All, GetSPInRandom());
    }

    private int[] GetSPInRandom()
    {
        return new int[] { 0, 1, 2, 3 };
    }

    [PunRPC]
    private void RPC_Spawn(int[] indexes)
    {
        if (indexes.Length < PhotonNetwork.PlayerList.Length)
            throw new System.Exception("Game has more players than spawnpoints available");

        else
            _spawnPoints[indexes[GetPlayerIndexInList()]].Spawn(_playerTank);
    }

    private int GetPlayerIndexInList()
    {
        int index = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.NickName != PhotonNetwork.NickName)
                index++;
            else
                return index;
        }

        throw new System.Exception("No such user in list");
    }

}
