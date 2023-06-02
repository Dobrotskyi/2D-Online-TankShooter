using System.Collections;
using UnityEngine.Networking;

public abstract class ObjectFromDBBuilder
{
    public ObjectFromDBBuilder() { }

    public PartData Build()
    {
        if (Verify())
            return MakePart();
        else throw new System.Exception("Data was not full");
    }

    public static IEnumerator GetSelectedByUser(ObjectFromDBBuilder builder, string url)
    {
        PHPCaller caller = new(url);
        yield return caller.MakeCallWithNickname(DBManager.LoginedUserName);
        while (caller.ResultStatus == UnityWebRequest.Result.InProgress)
            yield return null;
        if (caller.ResultStatus != UnityWebRequest.Result.Success)
            yield return new System.Exception("Loading data was failed");

        builder.ParseData(caller.Result);
        yield return null;
    }

    protected abstract bool Verify();
    protected abstract PartData MakePart();
    public abstract void ParseData(string[] result);
}
