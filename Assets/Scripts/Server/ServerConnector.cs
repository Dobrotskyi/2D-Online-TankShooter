using Photon.Pun;
using UnityEngine.SceneManagement;

public class ServerConnector : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (PhotonNetwork.NetworkClientState != Photon.Realtime.ClientState.Disconnected)
            PhotonNetwork.Disconnect();
        PhotonNetwork.NickName = DBManager.LoginedUserName;
        PhotonNetwork.SendRate = 80;
        PhotonNetwork.SerializationRate = 35;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("CreateLobbyMenu");
    }
}
