using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UserShoppingTransacHandler : MonoBehaviour
{
    protected abstract string URL { get; }
    [SerializeField] protected ItemTemplateFiller filler;
    public void UserPressed()
    {
        StartCoroutine(MakeTransaction());
    }

    private IEnumerator MakeTransaction()
    {
        if (ValidateTransaction() == false)
            yield break;

        PHPCaller caller = new PHPCaller(URL);
        Dictionary<string, string> parameters = new Dictionary<string, string>() { { "nickname", DBManager.LoginedUserName },
                                                                                    { "id", filler.PartData.Id.ToString() },
                                                                                    { "part_type", filler.PartData.GetType().Name } };
        yield return caller.MakeCallWithParameters(parameters);
        if (caller.ResultStatus == UnityEngine.Networking.UnityWebRequest.Result.Success)
            yield return FinishTransaction();
    }

    protected virtual bool ValidateTransaction() { return true; }
    protected abstract IEnumerator FinishTransaction();
}
