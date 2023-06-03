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
        yield return StartCoroutine(DBManager.AddToUsersItems(filler.PartData));
        GameObject.FindObjectOfType(typeof(Canvas)).GetComponent<ShopMenu>().ChangeContentWithoutCheck();
    }
}
