using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserBuyHandler : MonoBehaviour
{
    [SerializeField] private ItemTemplateFiller filler;
    public void UserBought()
    {
        StartCoroutine(DBManager.AddToUsersItems(filler.PartData));
    }
}
