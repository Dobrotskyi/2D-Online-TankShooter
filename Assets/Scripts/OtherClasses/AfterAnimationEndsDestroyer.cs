using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AfterAnimationEndsDestroyer : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(gameObject, gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
