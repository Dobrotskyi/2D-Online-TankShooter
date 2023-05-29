using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerDisplay;
    [SerializeField] private Button _playButton;

    private void Start()
    {
        if (DBManager.IsLogged())
        {
            _playerDisplay.text = $"Welcome, {DBManager.Nickname} ";
            _playButton.interactable = true;
        }
    }

    public void LoadRegisterPage() => SceneManager.LoadScene("RegisterMenu");
    public void LoadLoginPage() => SceneManager.LoadScene("LoginMenu");
    public void LoadGame() => SceneManager.LoadScene("Game");
}
