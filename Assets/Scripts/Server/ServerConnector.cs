using Photon.Pun;
using UnityEngine.SceneManagement;

public class ServerConnector : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Disconnected)
        PhotonNetwork.NickName = DBManager.LoginedUserName;
        PhotonNetwork.SendRate = 80;
        PhotonNetwork.SerializationRate = 35;
        PhotonNetwork.ConnectUsingSettings();
        //if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.ConnectedToMasterServer)
        //    OnConnectedToMaster();
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("CreateLobbyMenu");
    }
}
