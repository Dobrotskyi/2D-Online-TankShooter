using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class UserBuyHandler : UserShoppingTransacHandler
{
    protected override string URL => DBManager.ADD_PART_TO_USER_URL;

    protected override IEnumerator FinishTransaction()
    {
        GameObject.FindObjectOfType(typeof(Canvas)).GetComponent<ShopMenu>().ChangeContentWithoutCheck();
        yield return null;
    }
}
