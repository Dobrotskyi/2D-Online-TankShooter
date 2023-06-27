using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuReturner : MonoBehaviour
{
    public void GoMeinMenu()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }
}
