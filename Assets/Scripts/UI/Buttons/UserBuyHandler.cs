using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserBuyHandler : UserShoppingTransacHandler
{
    protected override string URL => DBManager.ADD_PART_TO_USER_URL;
}
