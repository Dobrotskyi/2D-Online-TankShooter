using System.Collections;
using System.Collections.Generic;
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
        yield return MakeCallWithParameters(new Dictionary<string, string>() { { "nickname", nickname } });
    }

    public IEnumerator MakeCallWithParameters(Dictionary<string, string> parameters)
    {
        WWWForm form = new();

        foreach (var pair in parameters)
            form.AddField(pair.Key, pair.Value);

        UnityWebRequest uwr = UnityWebRequest.Post(_url, form);
        yield return uwr.SendWebRequest();

        ResultStatus = uwr.result;
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.result.ToString());
            Debug.Log(uwr.downloadHandler.text);
            uwr.Dispose();
            yield return new System.Exception(uwr.result.ToString());
            yield break;
        }

        string result = uwr.downloadHandler.text;
        if (result[0] != '0')
        {
            Debug.Log("Smth went wrong");
            Debug.Log(result);
            uwr.Dispose();
            yield return new System.Exception($"Smth went wrong: {result}");
            yield break;
        }
        uwr.Dispose();
        _result = result.Split(',');
        _result[0] = _result[0].Substring(1);
        Debug.Log("Call was finnished");
    }
}
