using UnityEngine;

public abstract class ResourceCrate : MonoBehaviour, IResourceCrate
{
    [SerializeField] protected Vector2 _minMaxCapacity = new Vector2(20, 20);
    protected int _capacity;

    public abstract void UseMeOn(Tank tank);

    public void OnEnable()
    {
        _capacity = Random.Range((int)_minMaxCapacity.x, (int)_minMaxCapacity.y);
    }

    protected void DestroyThis()
    {
        //Destroy(gameObject);
    }

}
