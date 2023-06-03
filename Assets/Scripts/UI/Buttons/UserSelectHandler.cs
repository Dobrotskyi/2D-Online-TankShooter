using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSelectHandler : UserShoppingTransacHandler
{
    protected override string URL => DBManager.SELECT_NEW_PART_URL;
}
