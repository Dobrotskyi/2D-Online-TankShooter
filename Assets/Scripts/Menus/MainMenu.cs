using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadRegisterPage() => SceneManager.LoadScene("RegisterMenu");
    public void LoadLoginPage() => SceneManager.LoadScene("LoginMenu");
    public void LoadGame() => SceneManager.LoadScene("Game");
}
