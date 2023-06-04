using System.Collections;
using UnityEngine;

public class UserBuyHandler : UserShoppingTransacHandler
{
    protected override string URL => DBManager.ADD_PART_TO_USER_URL;

    protected override IEnumerator FinishTransaction()
    {
        StartCoroutine(DBManager.UpdateMoneyAmt());
        GetComponent<Animator>().enabled = true;
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        yield return null;
    }
}
