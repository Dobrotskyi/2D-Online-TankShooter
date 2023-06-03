using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class UserShoppingTransacHandler : MonoBehaviour
{
    protected abstract string URL { get; }
    [SerializeField] private ItemTemplateFiller filler;
    public void UserPressed()
    {
        StartCoroutine(MakeTransaction());
    }

    private IEnumerator MakeTransaction()
    {
        PHPCaller caller = new PHPCaller(URL);
        Dictionary<string, string> parameters = new Dictionary<string, string>() { { "nickname", DBManager.LoginedUserName },
                                                                                    { "id", filler.PartData.Id.ToString() },
                                                                                    { "part_type", filler.PartData.GetType().Name } };
        yield return caller.MakeCallWithParameters(parameters);
        GameObject.FindObjectOfType(typeof(Canvas)).GetComponent<ShopMenu>().ChangeContentWithoutCheck();
    }
}
