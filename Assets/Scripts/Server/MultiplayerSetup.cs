using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _joinInputField;
    [SerializeField] private TMP_InputField _createRoomInputField;

    public void CreateRoom()
    {
        if (_createRoomInputField.text.Length == 0)
            throw new System.Exception("Name field for create room was empty");
        RoomOptions roomOptions = new();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(_createRoomInputField.text, roomOptions);
    }

    public void JoinRoom()
    {
        if (_joinInputField.text.Length == 0)
            throw new System.Exception("Name field for join room was empty");
        PhotonNetwork.JoinRoom(_joinInputField.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
