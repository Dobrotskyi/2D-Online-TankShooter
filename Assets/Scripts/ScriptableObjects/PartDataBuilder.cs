using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class PartDataBuilder
{
    protected static string PHP_URL;
    private string _name;
    private Sprite _sprite;

    public PartDataBuilder() { }

    public PartData Build()
    {
        if (Verify())
            return MakePart();
        else throw new System.Exception("Data was not full");
    }

    public static IEnumerator GetSelectedByUser(PartDataBuilder builder)
    {
        WWWForm form = new();
        form.AddField("nickname", DBManager.LoginedUserName);

        UnityWebRequest uwr = UnityWebRequest.Post(PHP_URL, form);
        yield return uwr.SendWebRequest();

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
        }
        else
        {
            yield return builder.ParseDataToBuilder(result);
        }
    }

    //private static IEnumerator MakeCallWithNickname(out string result, string url, string name)
    //{


    //}


    protected abstract bool Verify();
    protected abstract PartData MakePart();
    protected abstract IEnumerator ParseDataToBuilder(string result);
}
