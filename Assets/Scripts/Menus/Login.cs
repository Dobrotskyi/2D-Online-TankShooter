using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private const string LOGIN_URL = DBManager.LOGIN_URL;

    [SerializeField] private TMP_InputField _nicknameField;
    [SerializeField] private TMP_InputField _passwordField;
    [SerializeField] private Button _submitButton;
    [SerializeField] private Button _goMainMenuButton;

    public void MakeLoginCall()
    {
        StartCoroutine(TryLoggin());
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator TryLoggin()
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", _nicknameField.text);
        form.AddField("password", _passwordField.text);

        UnityWebRequest uwr = UnityWebRequest.Post(LOGIN_URL, form);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.result.ToString());
            yield break;
        }

        string result = uwr.downloadHandler.text;
        if (result[0] == '0')
        {
            DBManager.LoginedUserName = _nicknameField.text;
            DBManager.Money = int.Parse(result.Split("\t")[1]);
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log("Loggin failed");
            Debug.Log(result);
        }

        uwr.Dispose();
        yield break;

    }

    public void VerifyInputs()
    {
        _submitButton.interactable = (_nicknameField.text.Length >= DBManager.MIN_NAME_LENGTH
                                      && _passwordField.text.Length >= DBManager.MIN_PASSW_LENGTH
                                      && _nicknameField.text.Length < DBManager.MAX_NAME_LENGTH);
    }
}
