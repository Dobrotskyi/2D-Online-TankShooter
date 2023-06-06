using Photon.Pun;
using UnityEngine.SceneManagement;

public class ServerConnector : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Disconnected)
        PhotonNetwork.NickName = DBManager.LoginedUserName;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.ConnectUsingSettings();
        //if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.ConnectedToMasterServer)
        //    OnConnectedToMaster();
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("CreateLobbyMenu");
    }
}
