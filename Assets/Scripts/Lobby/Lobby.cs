using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private LobbyListItem _lobbyListItem;
    [SerializeField] private VerticalLayoutGroup _content;
    [SerializeField] private Button[] _buttons = new Button[2];
    [SerializeField] private Transform _notificationSpawnpoint;
    private Dictionary<string, LobbyListItem> _players = new();
    private PhotonView _view;

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        foreach (var player in _players.Values)
            if (player.Status == PlayerReadyStatus.Not_Ready)
            {
                NotificationFabric.Instance.DisplayNotification($"Some players are not ready", _notificationSpawnpoint.position, NotificationType.Fail, transform);
                return;
            }

        SceneManager.LoadScene("Game");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        AddToList(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        RemoveFromList(otherPlayer);
    }

    public void ReadyButtonPressed()
    {
        _players[PhotonNetwork.NickName].ChangeStatus();
        if (_players[PhotonNetwork.NickName].Status == PlayerReadyStatus.Ready)
            _buttons[0].transform.GetComponentInChildren<TextMeshProUGUI>().text = PlayerReadyStatus.Not_Ready.ToString().Replace("_", " ");
        else
            _buttons[0].transform.GetComponentInChildren<TextMeshProUGUI>().text = PlayerReadyStatus.Ready.ToString();
        _view.RPC("ChangePlayerStatus", RpcTarget.Others, PhotonNetwork.NickName);
    }

    [PunRPC]
    private void ChangePlayerStatus(string nickname) => _players[nickname].ChangeStatus();

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            _buttons[1].gameObject.SetActive(false);
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            AddToList(player);
        _view = GetComponent<PhotonView>();
    }

    private void AddToList(Photon.Realtime.Player player)
    {
        LobbyListItem item = Instantiate(_lobbyListItem, _content.transform);
        item.InitializeItem(player);
        _players[player.NickName] = item;
    }

    private void RemoveFromList(Photon.Realtime.Player player)
    {
        Destroy(_players[player.NickName].gameObject);
        _players.Remove(player.NickName);
    }
}
