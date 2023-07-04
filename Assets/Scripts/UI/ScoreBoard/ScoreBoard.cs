using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _scoreboardItem;
    private Dictionary<string, ScoreboardItem> _scoreboardPairs = new Dictionary<string, ScoreboardItem>();

    public int GetPlayerKills(string name)
    {
        if (_scoreboardPairs.ContainsKey(name))
            return _scoreboardPairs[name].Kills;
        return -1;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        AddToScoreboard(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        RemoveFromScoreboard(otherPlayer);
    }

    public void AddKillTo(string name)
    {
        _scoreboardPairs[name].AddKill();
        UpdateStandings();
    }

    private void UpdateStandings()
    {
        _scoreboardPairs = _scoreboardPairs.OrderByDescending(x => x.Value.Kills).ToDictionary(x => x.Key, x => x.Value);

        int i = 0;
        foreach (var item in _scoreboardPairs.Values)
            item.SetSiblingIndex(i++);
    }

    private void RemoveFromScoreboard(Photon.Realtime.Player player)
    {
        Destroy(_scoreboardPairs[player.NickName].gameObject);
        _scoreboardPairs.Remove(player.NickName);
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
}
