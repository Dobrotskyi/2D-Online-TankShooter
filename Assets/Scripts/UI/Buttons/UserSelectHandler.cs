using System;
using System.Collections;

public class UserSelectHandler : UserShoppingTransacHandler
{
    public static event Action SelectTransactionFinished;
    protected override string URL => DBManager.SELECT_NEW_PART_URL;
    protected override IEnumerator FinishTransaction()
    {
        yield return DBManager.MakeCallGetSelectedIDs();
        SelectTransactionFinished?.Invoke();
    }
}
