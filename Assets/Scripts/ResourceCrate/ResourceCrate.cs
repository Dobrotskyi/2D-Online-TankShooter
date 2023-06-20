using System;
using System.Collections;
using UnityEngine;

public abstract class ResourceCrate : MonoBehaviour, IResourceCrate
{
    public event Action Destroyed;

    [SerializeField] protected Vector2 _minMaxCapacity = new Vector2(20, 20);
    [SerializeField] private float _timeOfLifeInSec = 20f;
    protected int _capacity;

    public abstract void UseMeOn(Tank tank);

    public void OnEnable()
    {
        _capacity = UnityEngine.Random.Range((int)_minMaxCapacity.x, (int)_minMaxCapacity.y);
        StartCoroutine(TimeOfLifeExpired());
    }

    private IEnumerator TimeOfLifeExpired()
    {
        yield return new WaitForSeconds(_timeOfLifeInSec);
        DestroyThis();
    }

    protected void DestroyThis()
    {
        Destroyed?.Invoke();
        Destroy(gameObject);
    }
}
