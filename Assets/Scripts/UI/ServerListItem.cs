using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ServerListItem : MonoBehaviour
{
    public RoomInfo RoomInfo { get; private set; }

    [SerializeField] private TextMeshProUGUI _serverNameTxt;
    [SerializeField] private TextMeshProUGUI _playersAmt;

    public void SetInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _serverNameTxt.text = RoomInfo.Name;
        _playersAmt.text = $"{RoomInfo.PlayerCount}/{RoomInfo.MaxPlayers}";
    }
}
