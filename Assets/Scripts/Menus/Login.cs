using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private const string LOGIN_URL = DBManager.LOGIN_URL;

    [SerializeField] private TMP_InputField _nicknameField;
    [SerializeField] private TMP_InputField _passwordField;
    [SerializeField] private Button _submitButton;

    public void MakeLoginCall()
    {
        StartCoroutine(TryLoggin());
    }

    [System.Obsolete]
    public IEnumerator TryLoggin()
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", _nicknameField.text);
        form.AddField("password", _passwordField.text);

        WWW www = new WWW(LOGIN_URL, form);
        yield return www;

        if (www.text[0] == '0')
        {
            DBManager.Nickname = _nicknameField.text;
            DBManager.Money = int.Parse(www.text.Split("\t")[1]);
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log("Loggin failed");
            Debug.Log(www.text);
        }

        www.Dispose();
        yield break;

    }

    public void VerifyInputs()
    {
        _submitButton.interactable = (_nicknameField.text.Length >= DBManager.MIN_NAME_LENGTH
                                      && _passwordField.text.Length >= DBManager.MIN_PASSW_LENGTH
                                      && _nicknameField.text.Length < DBManager.MAX_NAME_LENGTH);
    }
}
