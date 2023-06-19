using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Registration : Authorization
{
    private const string REG_URL = DBManager.REG_URL;

    [SerializeField] private TMP_InputField _nicknameField;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private Button _submitButton;

    public void MakeRegisterCall()
    {
        StartCoroutine(Register());
    }

    public void VerifyInputs()
    {
        _submitButton.interactable = (_nicknameField.text.Length >= DBManager.MIN_NAME_LENGTH
                                      && _password.text.Length >= DBManager.MIN_PASSW_LENGTH
                                      && _nicknameField.text.Length < DBManager.MAX_NAME_LENGTH);
    }

    private IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", _nicknameField.text);
        form.AddField("password", _password.text);

        UnityWebRequest uwr = UnityWebRequest.Post(REG_URL, form);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.result.ToString());
            uwr.Dispose();
            yield break;
        }

        string result = uwr.downloadHandler.text;
        if (result == "0")
            DisplayMessage("Registration was succesful", _submitButton.transform.position, MessageType.Success);
        else
            DisplayMessage(result.Substring(1), _submitButton.transform.position, MessageType.Fail);

        uwr.Dispose();
        yield break;
    }
}