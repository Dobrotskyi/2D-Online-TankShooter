using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private LobbyListItem _lobbyListItem;
    [SerializeField] private VerticalLayoutGroup _content;
    [SerializeField] private Button[] _buttons = new Button[2];
    private Dictionary<string, LobbyListItem> _players = new();

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        AddToList(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        RemoveFromList(otherPlayer);
    }

    private void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
            _buttons[1].gameObject.SetActive(false);
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            AddToList(player);
    }

    private void AddToList(Photon.Realtime.Player player)
    {
        LobbyListItem item = Instantiate(_lobbyListItem, _content.transform);
        item.InitializeItem(player, PlayStatus.Not_Ready);
        _players[player.NickName] = item;
    }

    private void RemoveFromList(Photon.Realtime.Player player)
    {
        Destroy(_players[player.NickName].gameObject);
        _players.Remove(player.NickName);
    }
}
