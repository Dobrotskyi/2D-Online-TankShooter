using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UserBuyHandler))]
public class ItemBuyAnimationsPlayer : MonoBehaviour
{
    private UserBuyHandler _userBuyHandler;
    private Animator _animator;

    private void Start()
    {
        _userBuyHandler = GetComponent<UserBuyHandler>();
        _userBuyHandler.ItemWasBought += PlayAnimationOnBuy;
        _userBuyHandler.TransactionDeclined += PlayAnimationOnDecline;

        _animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        _userBuyHandler.ItemWasBought -= PlayAnimationOnBuy;
        _userBuyHandler.TransactionDeclined -= PlayAnimationOnDecline;
    }

    public void PlayAnimationOnBuy()
    {
        if (_animator.enabled)
            return;
        _animator.enabled = true;
        _animator.SetTrigger("Purchased");
        Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void PlayAnimationOnDecline()
    {
        if (_animator.enabled)
            return;
        _animator.enabled = true;
        _animator.SetTrigger("Declined");
        StartCoroutine(SetAnimatorDisabled(_animator.GetCurrentAnimatorStateInfo(0).length));
    }

    private IEnumerator SetAnimatorDisabled(float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        _animator.enabled = false;
    }

}
