using System.Collections;
using UnityEngine.Networking;

public abstract class ObjectFromDBBuilder
{
    protected abstract string PHP_URL { get; }

    public ObjectFromDBBuilder() { }

    public PartData Build()
    {
        if (Verify())
            return MakePart();
        else throw new System.Exception("Data was not full");
    }

    public static IEnumerator GetSelectedByUser(ObjectFromDBBuilder builder)
    {
        PHPCaller caller = new(builder.PHP_URL);
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
    protected abstract void ParseData(string[] result);
}
