using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerDisplay;
    [SerializeField] private Button[] _restrictedBtns;

    private void Start()
    {

#if UNITY_EDITOR
        if (DBManager.IsLogged() == false)
            DBManager.LoginedUserName = "admin1";
#endif

        if (DBManager.IsLogged())
        {
            _playerDisplay.text = $"Welcome, {DBManager.LoginedUserName} ";
            foreach (var b in _restrictedBtns)
                b.interactable = true;
            StartCoroutine(DBManager.MakeCallGetSelectedIDs());
        }
    }

    public void LoadRegisterPage() => SceneManager.LoadScene("RegisterMenu");
    public void LoadLoginPage() => SceneManager.LoadScene("LoginMenu");
    public void LoadGame() => SceneManager.LoadScene("LoadingScreen");
    public void LoadShop() => SceneManager.LoadScene("StoreMenu");
}
