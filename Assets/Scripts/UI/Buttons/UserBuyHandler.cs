using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UserBuyHandler : UserShoppingTransacHandler
{
    [SerializeField] private TextMeshProUGUI _coinsField;
    public event Action TransactionDeclined;
    public event Action ItemWasBought;
    protected override string URL => DBManager.ADD_PART_TO_USER_URL;

    protected override IEnumerator FinishTransaction()
    {
        StartCoroutine(DBManager.UpdateMoneyAmt());
        ItemWasBought?.Invoke();
        yield return null;
    }

    protected override bool ValidateTransaction()
    {
        if (DBManager.Money < int.Parse(_coinsField.text))
        {
            TransactionDeclined?.Invoke();
            return false;
        }
        return true;
    }
}
