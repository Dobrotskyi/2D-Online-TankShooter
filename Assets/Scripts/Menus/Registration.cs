using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class Registration : MonoBehaviour
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
        Debug.Log(_nicknameField.text.Length);
        Debug.Log(_password.text.Length);
        _submitButton.interactable = (_nicknameField.text.Length >= 8 && _password.text.Length >= 8);
    }

    [System.Obsolete]
    private IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("nickname", _nicknameField.text);
        form.AddField("password", _password.text);

        string url = "http://localhost/topdowntankshooter/register.php";
        WWW www = new WWW(url, form);
        yield return www;
        if (www.text == "0")
        {
            Debug.Log("user was connected succesfully");
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log($"Smth was wrong while trying to register user# {www.text}");
        }
        www.Dispose();
        yield break;
    }
}
