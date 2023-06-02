using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuReturner : MonoBehaviour
{
    public void GoMeinMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
