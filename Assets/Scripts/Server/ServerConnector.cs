using Photon.Pun;
using UnityEngine.SceneManagement;

public class ServerConnector : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Disconnected)
        PhotonNetwork.ConnectUsingSettings();
        //if (PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.ConnectedToMasterServer)
        //    OnConnectedToMaster();
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("CreateLobbyMenu");
    }
}
