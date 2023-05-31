using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        while (caller.Result == null)
            yield return null;

        builder.ParseDataToBuilder(caller.Result);
        yield return null;
    }

    protected abstract bool Verify();
    protected abstract PartData MakePart();
    protected abstract void ParseDataToBuilder(string[] result);
}
