using TMPro;
using UnityEngine;
using Photon.Pun;

public class JoinRoomButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _serverNameField;
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_serverNameField.text);
    }
}
