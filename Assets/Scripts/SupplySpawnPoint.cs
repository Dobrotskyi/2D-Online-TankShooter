using UnityEngine;

public class SupplySpawnPoint : MonoBehaviour
{
    public bool IsEmpty { get; private set; }
    private ResourceCrate _crate;

    public bool Spawn(ResourceCrate crate)
    {
        if (IsEmpty == false)
            return false;


        IsEmpty = false;

        _crate = Instantiate(crate, transform.position, Quaternion.identity);
        _crate.Destroyed += EmptySP;

        return _crate;
    }

    private void Awake()
    {
        IsEmpty = true;
    }

    private void EmptySP()
    {
        IsEmpty = true;
        _crate.Destroyed -= EmptySP;
    }

    public void SetEmpty()
    {
        IsEmpty = true;
    }
}
