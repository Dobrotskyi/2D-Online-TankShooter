using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class PHPCaller
{
    private string _url;
    private string[] _result = null;

    public string[] Result => _result;
    public UnityWebRequest.Result ResultStatus { private set; get; }

    public PHPCaller(string url)
    {
        _url = url;
        ResultStatus = UnityWebRequest.Result.InProgress;
    }

    public IEnumerator MakeCallWithNickname(string nickname)
    {
        WWWForm form = new();
        form.AddField("nickname", DBManager.LoginedUserName);

        UnityWebRequest uwr = UnityWebRequest.Post(_url, form);
        yield return uwr.SendWebRequest();

        ResultStatus = uwr.result;
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.result.ToString());
            Debug.Log(uwr.downloadHandler.text);
            yield break;
        }

        string result = uwr.downloadHandler.text;
        if (result[0] != '0')
        {
            Debug.Log("Smth went wrong");
            Debug.Log(result);
            uwr.Dispose();
            yield return new System.Exception();
            yield break;
        }
        uwr.Dispose();
        _result = result.Split(',');
        _result[0] = _result[0].Substring(1);
        Debug.Log("Call was finnished");
    }
}
