using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public abstract class ResourceCrate : MonoBehaviour, IResourceCrate
{
    public event Action Destroyed;

    private float _fadingTime;
    [SerializeField] protected Vector2 _minMaxCapacity = new Vector2(20, 20);
    [SerializeField] private float _timeOfLifeInSec = 20f;
    private Animator _animator;
    protected int _capacity;

    public abstract void UseMeOn(Tank tank);

    public void OnEnable()
    {
        _animator = GetComponent<Animator>();
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
            if (clip.name == "Fading")
                _fadingTime = clip.length;

        _capacity = UnityEngine.Random.Range((int)_minMaxCapacity.x, (int)_minMaxCapacity.y);
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(TimeOfLifeExpired());
    }

    private IEnumerator TimeOfLifeExpired()
    {
        yield return new WaitForSeconds(_timeOfLifeInSec - _fadingTime);
        _animator.SetBool("Fading", true);
        yield return new WaitForSeconds(_fadingTime);
        DestroyThis();
    }

    protected void DestroyThis()
    {
        GetComponent<PhotonView>().RPC("RPC_Destroy", RpcTarget.All);
    }

    [PunRPC]
    protected void RPC_Destroy()
    {
        Destroyed?.Invoke();
        Destroy(gameObject);
    }
}
