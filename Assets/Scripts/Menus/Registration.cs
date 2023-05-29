using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Registration : MonoBehaviour
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
        Debug.Log(_password.text);
        Debug.Log(_password.text != _password.placeholder.GetComponent<TextMeshProUGUI>().text);
        _submitButton.interactable = (_nicknameField.text.Length >= DBManager.MIN_NAME_LENGTH
                                      && _password.text.Length >= DBManager.MIN_PASSW_LENGTH
                                      && _nicknameField.text.Length < DBManager.MAX_NAME_LENGTH);
    }

    [System.Obsolete]
    private IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", _nicknameField.text);
        form.AddField("password", _password.text);

        WWW www = new WWW(REG_URL, form);
        yield return www;
        if (www.text == "0")
        {
            Debug.Log("user was connected succesfully");
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log($"Smth was wrong while trying to register user#");
            Debug.Log(www.text);
        }
        www.Dispose();
        yield break;
    }
}
