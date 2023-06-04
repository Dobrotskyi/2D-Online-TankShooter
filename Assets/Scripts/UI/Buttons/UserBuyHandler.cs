using System.Collections;
using TMPro;
using UnityEngine;

public class UserBuyHandler : UserShoppingTransacHandler
{
    [SerializeField] private TextMeshProUGUI _coinsField;
    protected override string URL => DBManager.ADD_PART_TO_USER_URL;

    protected override IEnumerator FinishTransaction()
    {
        StartCoroutine(DBManager.UpdateMoneyAmt());
        GetComponent<Animator>().enabled = true;
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        yield return null;
    }

    protected override bool ValidateTransaction()
    {
        if (DBManager.Money < int.Parse(_coinsField.text))
            return false;
        return true;
    }
}
