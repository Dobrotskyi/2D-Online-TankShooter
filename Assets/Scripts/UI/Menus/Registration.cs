using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Registration : Authorization
{
    [SerializeField] private TMP_InputField _nicknameField;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private Button _submitButton;

    public void MakeRegisterCall()
    {
        StartCoroutine(Register());
    }

    public void VerifyInputs()
    {
        _submitButton.interactable = (_nicknameField.text.Length >= MIN_NAME_LENGTH
                                      && _password.text.Length >= MIN_PASSW_LENGTH
                                      && _nicknameField.text.Length < MAX_NAME_LENGTH);
    }

    private IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", _nicknameField.text);
        form.AddField("password", _password.text);

        UnityWebRequest uwr = UnityWebRequest.Post(DBManager.ServerURLS.REG_URL, form);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.result.ToString());
            uwr.Dispose();
            yield break;
        }

        string result = uwr.downloadHandler.text;
        if (result == "0")
        {
            DisplayMessage("Registration was succesful", _submitButton.transform.position, NotificationType.Success);
            SceneManager.LoadScene("LoginMenu");
        }
        else
            DisplayMessage(result.Substring(1), _submitButton.transform.position, NotificationType.Fail);

        uwr.Dispose();
        yield break;
    }
}
