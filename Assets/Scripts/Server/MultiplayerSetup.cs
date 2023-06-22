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
    private const int MAX_INPUT_LENGTH = 8;

    public void CreateRoom()
    {
        if (ValidateInput(_createRoomInputField.text) == false)
            return;

        RoomOptions roomOptions = new();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(_createRoomInputField.text, roomOptions);
    }

    public void JoinRoom()
    {
        if (ValidateInput(_joinInputField.text))
            PhotonNetwork.JoinRoom(_joinInputField.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    private bool ValidateInput(string input)
    {
        if (input.Length == 0)
        {
            DisplayError("Field was empty", _spawnNotificationPos);
            return false;
        }

        if (input.Length > MAX_INPUT_LENGTH)
        {
            DisplayError("To many characters in input", _spawnNotificationPos);
            return false;
        }
        return true;
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
