using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserBuyHandler : MonoBehaviour
{
    [SerializeField] private ItemTemplateFiller filler;
    public void UserBought()
    {
        StartCoroutine(MakeTransaction());
    }

    private IEnumerator MakeTransaction()
    {
        yield return StartCoroutine(AddToUserItems(filler.PartData));
        GameObject.FindObjectOfType(typeof(Canvas)).GetComponent<ShopMenu>().ChangeContentWithoutCheck();
    }

    private IEnumerator AddToUserItems(PartData _part)
    {
        PHPCaller caller = new PHPCaller(DBManager.ADD_PART_TO_USER_URL);
        Dictionary<string, string> parameters = new Dictionary<string, string>() { { "nickname", DBManager.LoginedUserName }, { "id", _part.Id.ToString() }, { "part_type", _part.GetType().Name } };
        yield return caller.MakeCallWithParameters(parameters);
    }
}
