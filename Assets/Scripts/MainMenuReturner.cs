using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuReturner : MonoBehaviour
{
    public void GoMeinMenu()
    {
        if (SceneManager.GetActiveScene().name == "Game")
            PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }
}
