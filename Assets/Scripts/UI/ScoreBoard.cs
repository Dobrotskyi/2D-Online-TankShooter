using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _scoreboardItem;
    Dictionary<string, ScoreboardItem> _scoreboardPairs = new Dictionary<string, ScoreboardItem>();

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        AddToScoreboard(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        RemoveFromeScoreboard(otherPlayer);
    }

    public void UpdateScoreboard(string name)
    {
        _scoreboardPairs[name].AddKill();
    }

    private void Start()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            AddToScoreboard(player);
    }

    private void AddToScoreboard(Photon.Realtime.Player player)
    {
        ScoreboardItem scoreboardItem = Instantiate(_scoreboardItem, transform).GetComponent<ScoreboardItem>();
        scoreboardItem.Initialize(player);
        _scoreboardPairs[player.NickName] = scoreboardItem;
    }

    private void RemoveFromeScoreboard(Photon.Realtime.Player player)
    {
        Destroy(_scoreboardPairs[player.NickName].gameObject);
        _scoreboardPairs.Remove(player.NickName);
    }
}
