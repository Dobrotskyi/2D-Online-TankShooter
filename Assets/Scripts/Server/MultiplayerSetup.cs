using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class MultiplayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _notification;
    [SerializeField] private TMP_InputField _joinInputField;
    [SerializeField] private TMP_InputField _createRoomInputField;
    [SerializeField] private Vector3 _spawnNotificationPos;

    public void CreateRoom()
    {
        if (_createRoomInputField.text.Length == 0)
        {
            DisplayError("Name field for creating was empty", _spawnNotificationPos);
            return;
        }
        RoomOptions roomOptions = new();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(_createRoomInputField.text, roomOptions);
    }

    public void JoinRoom()
    {
        if (_joinInputField.text.Length == 0)
        {
            DisplayError("Name field for joining was empty", _spawnNotificationPos);
            return;
        }
        PhotonNetwork.JoinRoom(_joinInputField.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    private void DisplayError(string text, Vector3 pos)
    {
        GameObject notification = Instantiate(_notification);
        notification.GetComponent<Notification>().DisplayNotification(text);
        notification.transform.SetParent(transform);
        notification.transform.localPosition = pos;
        notification.transform.localScale = Vector3.one;
    }
}
