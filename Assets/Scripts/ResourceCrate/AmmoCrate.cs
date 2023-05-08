using UnityEngine;

public class AmmoCrate : MonoBehaviour, IResourceCrate
{
    [SerializeField] public int _capacity = 10;

    public void UseMeOn(Tank tank)
    {
        tank.RestoreAmmo(_capacity);
    }
}
