using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : Authorization
{
    [SerializeField] private TMP_InputField _nicknameField;
    [SerializeField] private TMP_InputField _passwordField;
    [SerializeField] private Button _submitButton;

    public void MakeLoginCall()
    {
        StartCoroutine(TryLoggin());
    }

    public IEnumerator TryLoggin()
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", _nicknameField.text);
        form.AddField("password", _passwordField.text);

        UnityWebRequest uwr = UnityWebRequest.Post(DBManager.ServerURLS.LOGIN_URL, form);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.result.ToString());
            DisplayMessage(uwr.result.ToString(), _submitButton.transform.position, MessageType.Fail);
            uwr.Dispose();
            yield break;
        }

        string result = uwr.downloadHandler.text;
        if (result[0] == '0')
        {
            DBManager.LoginedUserName = _nicknameField.text;
            DBManager.Money = int.Parse(result.Split("\t")[1]);
            Debug.Log(DBManager.Money);
            uwr.Dispose();
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            DisplayMessage(uwr.downloadHandler.text, _submitButton.transform.position, MessageType.Fail);
            Debug.Log(result);
        }

        uwr.Dispose();
    }

    public void VerifyInputs()
    {
        _submitButton.interactable = (_nicknameField.text.Length >= MIN_NAME_LENGTH
                                      && _passwordField.text.Length >= MIN_PASSW_LENGTH
                                      && _nicknameField.text.Length < MAX_NAME_LENGTH);
    }
}
